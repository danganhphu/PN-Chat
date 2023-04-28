﻿using Microsoft.IdentityModel.Tokens;
using PNChatServer.Data;
using PNChatServer.Dto;
using PNChatServer.Models;
using PNChatServer.Repository;
using PNChatServer.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PNChatServer.Service
{
    public class AuthService : IAuthService
    {
        protected readonly DbChatContext chatContext;

        public AuthService(DbChatContext chatContext)
        {
            this.chatContext = chatContext;
        }

        /// <summary>
        /// Đăng nhập hệ thống
        /// </summary>
        /// <param name="user">Thông tin tài khoản người dùng</param>
        /// <returns>AccessToken</returns>
        public AccessToken Login(User user)
        {
            string passCheck = DataHelpers.HashSHA256($"{user.UserName}_{user.Password}");
            User userExist = chatContext.Users
                .Where(x => x.UserName.Equals(user.UserName) && x.Password.Equals(passCheck))
                .FirstOrDefault();

            if (userExist == null)
                throw new ArgumentException("Tài khoản hoặc mật khẩu không đúng");

            userExist.LastLogin = DateTime.Now;
            chatContext.SaveChanges();

            DateTime expirationDate = DateTime.Now.Date.AddMinutes(EnviConfig.ExpirationInMinutes);
            long expiresAt = (long)(expirationDate - new DateTime(1970, 1, 1)).TotalSeconds;

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(EnviConfig.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, userExist.Code),
                    new Claim(ClaimTypes.Name, userExist.UserName),
                    new Claim(ClaimTypes.Expiration, expiresAt.ToString())

                }),
                Expires = expirationDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            return new AccessToken
            {
                User = userExist.Code,
                FullName = userExist.FullName,
                Avatar = userExist.Avatar,
                Token = jwtTokenHandler.WriteToken(token),

            };
        }

        /// <summary>
        /// Đăng ký tài khoản người dùng
        /// </summary>
        /// <param name="user">Thông tin tài khoản</param>
        public void SignUp(User user)
        {
            if (chatContext.Users.Any(x => x.UserName.Equals(user.UserName)))
                throw new ArgumentException("Tài khoản đã tồn tại");

            User newUser = new User()
            {
                Code = Guid.NewGuid().ToString("N"),
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Password = DataHelpers.HashSHA256($"{user.UserName}_{user.Password}"),
                Avatar = Constants.AVATAR_DEFAULT,
            };

            chatContext.Users.Add(newUser);
            chatContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật thông tin hubconnection. Sử dụng khi thông báo riêng cho từng cá nhân.
        /// </summary>
        /// <param name="userSession">User hiện tại đang đăng nhập</param>
        /// <param name="key">HubConnection</param>
        public void PutHubConnection(string userSession, string key)
        {
            User user = chatContext.Users
                .FirstOrDefault(x => x.Code.Equals(userSession));

            if (user != null)
            {
                user.CurrentSession = key;
                chatContext.SaveChanges();
            }
        }

        
    }
}
