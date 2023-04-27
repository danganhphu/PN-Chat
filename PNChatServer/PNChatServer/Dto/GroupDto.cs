﻿using PNChatServer.Models;

namespace PNChatServer.Dto
{
    public class GroupDto
    {
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// single: chat 1-1
        /// multi: chat 1-n
        /// </summary>
        public string Type { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        public string? Name { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime LastActive { get; set; }

        public List<UserDto>? Users { get; set; }

        public MessageDto? LastMessage { get; set; }
    }
}
