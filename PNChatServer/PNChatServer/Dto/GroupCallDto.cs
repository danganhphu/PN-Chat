namespace PNChatServer.Dto
{
    public class GroupCallDto
    {
        public string Code { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? Name { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime LastActive { get; set; }

        public List<CallDto>? Calls { get; set; }
        public CallDto? LastCall { get; set; }

    }
}
