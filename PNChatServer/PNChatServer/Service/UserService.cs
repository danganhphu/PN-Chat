﻿using Microsoft.EntityFrameworkCore;
using PNChatServer.Data;
using PNChatServer.Dto;
using PNChatServer.Models;
using PNChatServer.Repository;
using PNChatServer.Utils;

namespace PNChatServer.Service
{
    public class UserService : IUserService
    {
        protected readonly DbChatContext chatContext;
        protected readonly IWebHostEnvironment webHostEnvironment;

        public UserService(DbChatContext chatContext, IWebHostEnvironment webHostEnvironment)
        {
            this.chatContext = chatContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Lấy thông tin cá nhân của user
        /// </summary>
        /// <param name="userCode">User hiện tại đang đăng nhập</param>
        /// <returns>Thông tin user</returns>
        public async Task<UserDto> GetProfile(string userCode)
        {
            return await chatContext.Users
                    .Where(x => x.Code.Equals(userCode))
                    .Select(x => new UserDto()
                    {
                        Code = x.Code,
                        FullName = x.FullName,
                        Address = x.Address,
                        Avatar = x.Avatar,
                        Email = x.Email,
                        Gender = x.Gender,
                        Phone = x.Phone,
                        Dob = x.Dob
                    }).FirstOrDefaultAsync();
        }


        /// <summary>
        /// Cập nhật thông tin cá nhân
        /// </summary>
        /// <param name="userCode">User hiện tại đang đăng nhập</param>
        /// <param name="user">Thông tin user</param>
        /// <returns></returns>
        public async Task<UserDto> UpdateProfile(string userCode, UserDto user)
        {
            User us = await chatContext.Users
                    .FirstOrDefaultAsync(x => x.Code.Equals(userCode));

            if (us != null)
            {
                us.FullName = user.FullName;
                us.Dob = user.Dob;
                us.Email = user.Email;

                if (user.Avatar.Contains("data:image/png;base64,"))
                {
                    string pathAvatar = $"Resource/Avatar/{Guid.NewGuid().ToString("N")}";
                    string pathFile = Path.Combine(webHostEnvironment.ContentRootPath, pathAvatar);
                    DataHelpers.Base64ToImage(user.Avatar.Replace("data:image/png;base64,", ""), pathFile);
                    us.Avatar = user.Avatar = pathAvatar;
                }

                us.Address = user.Address;
                us.Phone = user.Phone;
                us.Gender = user.Gender;

                await chatContext.SaveChangesAsync();
            }
            return user;
        }

        /// <summary>
        /// Thêm mới liên hệ
        /// </summary>
        /// <param name="userCode">User hiện tại đang đăng nhập</param>
        /// <param name="user">Thông tin liên hệ</param>
        public async Task AddContact(string userCode, UserDto user)
        {
            Contact contact = new Contact()
            {
                UserCode = userCode,
                ContactCode = user.Code,
                Created = DateTime.Now
            };
            await chatContext.Contacts.AddAsync(contact);

            await chatContext.SaveChangesAsync();
        }

        /// <summary>
        /// Lấy danh sách liên hệ của user
        /// </summary>
        /// <param name="userCode">User hiện tại đang đăng nhập</param>
        /// <returns>Danh sách liên hệ</returns>
        public async Task<List<UserDto>> GetContact(string userCode)
        {
            return await chatContext.Contacts
                     .Where(x => x.UserCode.Equals(userCode) || x.ContactCode.Equals(userCode))
                     .OrderBy(x => x.UserContact.FullName)
                     .Select(x => new UserDto()
                     {
                         Avatar = x.UserContact.Avatar,
                         Code = x.UserContact.Code,
                         FullName = x.UserContact.FullName,
                         Address = x.UserContact.Address,
                         Dob = x.UserContact.Dob,
                         Email = x.UserContact.Email,
                         Gender = x.UserContact.Gender,
                         Phone = x.UserContact.Phone
                     }).ToListAsync();
        }

        /// <summary>
        /// Tìm kiếm liên hệ.
        /// </summary>
        /// <param name="userCode">User hiện tại đang đăng nhập</param>
        /// <param name="keySearch">Từ khóa tìm kiếm</param>
        /// <returns></returns>
        public async Task<List<UserDto>> SearchContact(string userCode, string keySearch)
        {
            if (string.IsNullOrWhiteSpace(keySearch))
                return new List<UserDto>();

            List<ContactDto> friends = await chatContext.Contacts
                   .Where(x => x.UserCode.Equals(userCode) || x.ContactCode.Equals(userCode))
                   .Select(x => new ContactDto()
                   {
                       ContactCode = x.ContactCode,
                       UserCode = x.UserCode
                   }).ToListAsync();

            List<UserDto> users = await chatContext.Users
                .Where(x => !x.Code.Equals(userCode))
                .Where(x => x.FullName.Contains(keySearch) || x.Phone.Contains(keySearch) || x.Email.Contains(keySearch))
                .OrderBy(x => x.FullName)
                .Select(x => new UserDto()
                {
                    Avatar = x.Avatar,
                    Code = x.Code,
                    FullName = x.FullName,
                }).ToListAsync();

            users.ForEach(x =>
            {
                if (friends.Any(y => y.UserCode.Equals(x.Code) || y.ContactCode.Equals(x.Code)))
                    x.IsFriend = true;
            });

            // Loại bỏ liên hệ đã có
            return users.FindAll(x => !x.IsFriend);
        }

        
    }
}
