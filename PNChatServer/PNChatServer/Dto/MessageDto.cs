using PNChatServer.Models;

namespace PNChatServer.Dto
{
    public class MessageDto
    {
        public long Id { get; set; }

        public string Type { get; set; } = string.Empty;

        public string GroupCode { get; set; } = string.Empty;

        public string? Content { get; set; }

        public string? Path { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public string SendTo { get; set; } = string.Empty;

        public UserDto? UserCreatedBy { get; set; }

        public List<IFormFile>? Attachments { get; set; }
    }
}
