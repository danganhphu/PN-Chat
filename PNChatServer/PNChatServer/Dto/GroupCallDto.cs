namespace PNChatServer.Dto
{
    public class GroupCallDto
    {
        public string Code { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? Name { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime LastActive { get; set; }

        public List<CallDto> Calls { get; set; } = new List<CallDto>();
        public CallDto LastCall { get; set; } = null!;

    }
}
