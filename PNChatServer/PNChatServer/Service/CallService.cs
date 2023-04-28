using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PNChatServer.Data;
using PNChatServer.Dto;
using PNChatServer.Hubs;
using PNChatServer.Models;
using PNChatServer.Repository;
using PNChatServer.Utils;
using RestSharp;

namespace PNChatServer.Service
{
    public class CallService : ICallService
    {
        protected readonly DbChatContext chatContext;
        protected readonly IWebHostEnvironment webHostEnvironment;
        private IHubContext<ChatHub> chatHub;

        public CallService(DbChatContext chatContext, IWebHostEnvironment webHostEnvironment, IHubContext<ChatHub> chatHub)
        {
            this.chatContext = chatContext;
            this.webHostEnvironment = webHostEnvironment;
            this.chatHub = chatHub;
        }

        /// <summary>
        /// Danh sách lịch sử cuộc gọi
        /// </summary>
        /// <param name="userSession">User hiện tại đang đăng nhập</param>
        /// <returns>Danh sách lịch sử cuộc gọi</returns>
        public List<GroupCallDto> GetCallHistory(string userSession)
        {
            List<GroupCallDto> groupCalls = chatContext.GroupCalls
                     .Where(x => x.Calls.Any(y => y.UserCode.Equals(userSession)))
                     .Select(x => new GroupCallDto()
                     {
                         Code = x.Code,
                         Name = x.Name,
                         Avatar = x.Avatar,
                         LastActive = x.LastActive,
                         Calls = x.Calls.OrderByDescending(y => y.Created)
                             .Select(y => new CallDto()
                             {
                                 UserCode = y.UserCode,
                                 User = new UserDto()
                                 {
                                     FullName = y.User.FullName,
                                     Avatar = y.User.Avatar
                                 },
                                 Created = y.Created,
                                 Status = y.Status
                             }).ToList()
                     }).ToList();
            groupCalls.ForEach(grpCall =>
            {
                /// hiển thị tên cuộc gọi là người trong cuộc thoại (không phải người đang đang nhập)
                /// VD; A gọi cho B => Màn hình A sẽ nhìn trên danh sách cuộc gọi tên B và ngược lại.
                var us = grpCall.Calls.FirstOrDefault(x => !x.UserCode.Equals(userSession));
                grpCall.Name = us?.User.FullName;
                grpCall.Avatar = us?.User.Avatar;

                grpCall.LastCall = grpCall.Calls.FirstOrDefault()!;
            });

            return groupCalls.OrderByDescending(x => x.LastActive).ToList();
        }

        /// <summary>
        /// Thông tin chi tiết lịch sử cuộc gọi
        /// </summary>
        /// <param name="userSession">User hiện tại đang đăng nhập</param>
        /// <param name="groupCallCode">Mã cuộc gọi</param>
        /// <returns>Danh sách cuộc gọi</returns>
        public List<CallDto> GetHistoryById(string userSession, string groupCallCode)
        {
            string friend = chatContext.Calls
                .Where(x => x.GroupCallCode.Equals(groupCallCode) && x.UserCode != userSession)
                .Select(x => x.UserCode)
                .FirstOrDefault();

            return chatContext.Calls
                .Where(x => x.UserCode.Equals(userSession) && x.GroupCallCode.Equals(groupCallCode))
                .OrderByDescending(x => x.Created)
                .Select(x => new CallDto()
                {
                    Status = x.Status,
                    Created = x.Created,
                    UserCode = friend
                })
                .ToList();
        }

        /// <summary>
        /// Thực hiện cuộc gọi
        /// </summary>
        /// <param name="userSession">User hiện tại đang đăng nhập</param>
        /// <param name="callTo">Người được gọi</param>
        /// <returns>Đường link truy câp cuộc gọi</returns>
        public string Call(string userSession, string callTo)
        {
            #region Gọi API tạo room - daily.co
            var url = "https://api.daily.co/";  
            var client = new RestClient(url);
           
            var request = new RestRequest("v1/rooms", Method.Post);
            
            request.AddHeader("Authorization", $"Bearer {EnviConfig.DailyToken}");
            RestResponse response = client.ExecutePost(request);
            DailyResponse dailyRoomResp = JsonConvert.DeserializeObject<DailyResponse>(response.Content);
            #endregion

            // Lấy thông tin lịch sử cuộc gọi đã gọi cho user
            string grpCallCode = chatContext.GroupCalls
                       .Where(x => x.Type.Equals(Constants.GroupType.SINGLE))
                       .Where(x => x.Calls.Any(y => y.UserCode.Equals(userSession) &&
                                   x.Calls.Any(y => y.UserCode.Equals(callTo))))
                       .Select(x => x.Code)
                       .FirstOrDefault();

            GroupCall groupCall = chatContext.GroupCalls.FirstOrDefault(x => x.Code.Equals(grpCallCode));
            DateTime dateNow = DateTime.Now;

            User userCallTo = chatContext.Users.FirstOrDefault(x => x.Code.Equals(callTo));

            // Kiểm tra lịch sử cuộc gọi đã tồn tại hay chưa. Nếu chưa => tạo nhóm gọi mới.
            if (groupCall == null)
            {
                groupCall = new GroupCall()
                {
                    Code = Guid.NewGuid().ToString("N"),
                    Created = dateNow,
                    CreatedBy = userSession,
                    Type = Constants.GroupType.SINGLE,
                    LastActive = dateNow
                };
                chatContext.GroupCalls.Add(groupCall);
            }

            /// Thêm danh sách thành viên trong cuộc gọi. Mặc định người gọi trạng thái OUT_GOING
            /// Người được gọi trạng thái MISSED. Nếu người được gọi join vào => CHuyển trạng thái IN_COMING

            List<Call> calls = new List<Call>(){
                new Call()
                {
                    GroupCallCode = groupCall.Code,
                    UserCode = userSession,
                    Status =Constants.CallStatus.OUT_GOING,
                    Created = dateNow,
                    Url =dailyRoomResp.url,
                },
                new Call()
                {
                    GroupCallCode = groupCall.Code,
                    UserCode = userCallTo.Code,
                    Status =Constants.CallStatus.MISSED,
                    Created = dateNow,
                    Url = dailyRoomResp.url,
                }
            };

            chatContext.Calls.AddRange(calls);
            chatContext.SaveChanges();

            ///Truyền thông tin realtime cuộc gọi. Thông tin hubConnection của user.
            if (!string.IsNullOrWhiteSpace(userCallTo.CurrentSession))
                chatHub.Clients.Client(userCallTo.CurrentSession).SendAsync("callHubListener", dailyRoomResp.url);

            return dailyRoomResp.url;
        }

        /// <summary>
        /// Tham gia cuộc gọi. Cập nhật trạng thái cuộc gọi thành IN_COMING
        /// </summary>
        /// <param name="userSession">User hiện tại đang đăng nhập</param>
        /// <param name="url">Đường dẫn truy cập video call</param>
        public void JoinVideoCall(string userSession, string url)
        {
            Call call = chatContext.Calls
                .Where(x => x.UserCode.Equals(userSession) && x.Url.Equals(url))
                .FirstOrDefault();

            if (call != null)
            {
                call.Status = Constants.CallStatus.IN_COMMING;
                chatContext.SaveChanges();
            }
        }


        /// <summary>
        /// Hủy cuộc gọi
        /// </summary>
        /// <param name="userCode">User hiện tại đang đăng nhập</param>
        /// <param name="url">Đường dẫn truy cập video call</param>
        public void CancelVideoCall(string userSession, string url)
        {
            string urlCall = chatContext.Calls
                .Where(x => x.UserCode.Equals(userSession) && x.Url.Equals(url))
                .Select(x => x.Url)
                .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(urlCall))
            {
                try
                {
                    #region gọi API xóa đường dẫn video call trên daily
                    var urlDelete = "https://api.daily.co/";
                    var client = new RestClient(urlDelete);

                    var request = new RestRequest($"v1/rooms/{urlCall.Split('/').Last()}", Method.Delete);
                    request.AddHeader("Authorization", $"Bearer {EnviConfig.DailyToken}");
                    RestResponse response = client.Execute(request);
                    #endregion
                }
                catch (Exception ex) { }
            }
        }
    }
}
