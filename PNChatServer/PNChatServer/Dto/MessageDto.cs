using PNChatServer.Models;

namespace PNChatServer.Dto
{
    public class MessageDto
    {
        public long Id { get; set; }

        public string Type { get; set; } = null!;

        public string GroupCode { get; set; } = null!;

        public string? Content { get; set; }

        public string? Path { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = null!;

        public string SendTo { get; set; } = null!;

        public UserDto UserCreatedBy { get; set; } = null!;

        public List<IFormFile>? Attachments { get; set; }
    }
}
