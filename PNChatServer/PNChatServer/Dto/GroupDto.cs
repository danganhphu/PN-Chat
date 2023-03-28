using PNChatServer.Models;

namespace PNChatServer.Dto
{
    public class GroupDto
    {
        public string Code { get; set; } = null!;

        /// <summary>
        /// single: chat 1-1
        /// multi: chat 1-n
        /// </summary>
        public string Type { get; set; } = null!;

        public string? Avatar { get; set; }

        public string? Name { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime LastActive { get; set; }

        public List<UserDto> Users { get; set; } = new List<UserDto>();

        public MessageDto LastMessage { get; set; } = null!;
    }
}
