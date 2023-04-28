using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PNChatServer.Dto;
using PNChatServer.Repository;

namespace PNChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatBoardsController : ControllerBase
    {
        private IChatBoardService _chatBoardService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ChatBoardsController(IChatBoardService chatBoardService, IHttpContextAccessor contextAccessor)
        {
            _chatBoardService = chatBoardService;
            _contextAccessor = contextAccessor;
        }

        [Route("get-history")]
        [HttpGet]
        public IActionResult GetHistory()
        {
            ResponseAPI responseAPI = new ResponseAPI();

            try
            {
                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                responseAPI.Data = _chatBoardService.GetHistory(userSession);

                return Ok(responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.Message = ex.Message;
                return BadRequest(responseAPI);
            }
        }


        [Route("get-info")]
        [HttpGet]
        public IActionResult GetInfo(string groupCode, string contactCode)
        {
            ResponseAPI responseAPI = new ResponseAPI();

            try
            {
                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                responseAPI.Data = _chatBoardService.GetInfo(userSession, groupCode, contactCode);

                return Ok(responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.Message = ex.Message;
                return BadRequest(responseAPI);
            }
        }


        [Route("groups")]
        [HttpPost]
        public IActionResult AddGroup(GroupDto group)
        {
            ResponseAPI responseAPI = new ResponseAPI();

            try
            {
                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                _chatBoardService.AddGroup(userSession, group);

                return Ok(responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.Message = ex.Message;
                return BadRequest(responseAPI);
            }
        }


        [Route("update-group-avatar")]
        [HttpPut]
        public IActionResult UpdateGroupAvatar(GroupDto group)
        {
            ResponseAPI responseAPI = new ResponseAPI();

            try
            {
                responseAPI.Data = _chatBoardService.UpdateGroupAvatar(group);

                return Ok(responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.Message = ex.Message;
                return BadRequest(responseAPI);
            }
        }


        [Route("send-message")]
        [HttpPost]
        public IActionResult SendMessage([FromQuery] string groupCode)
        {
            ResponseAPI responseAPI = new ResponseAPI();

            try
            {
                string jsonMessage = HttpContext.Request.Form["data"]!;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };

                MessageDto message = JsonConvert.DeserializeObject<MessageDto>(jsonMessage, settings);
                message.Attachments = Request.Form.Files.ToList();

                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                _chatBoardService.SendMessage(userSession, groupCode, message);

                return Ok(responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.Message = ex.Message;
                return BadRequest(responseAPI);
            }
        }

        [Route("get-message-by-group/{groupCode}")]
        [HttpGet]
        public IActionResult GetMessageByGroup(string groupCode)
        {
            ResponseAPI responseAPI = new ResponseAPI();

            try
            {
                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                responseAPI.Data = _chatBoardService.GetMessageByGroup(userSession, groupCode);

                return Ok(responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.Message = ex.Message;
                return BadRequest(responseAPI);
            }
        }


        [Route("get-message-by-contact/{contactCode}")]
        [HttpGet]
        public IActionResult GetMessageByContact(string contactCode)
        {
            ResponseAPI responseAPI = new ResponseAPI();

            try
            {
                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                responseAPI.Data = _chatBoardService.GetMessageByContact(userSession, contactCode);

                return Ok(responseAPI);
            }
            catch (Exception ex)
            {
                responseAPI.Message = ex.Message;
                return BadRequest(responseAPI);
            }
        }
    }
}
