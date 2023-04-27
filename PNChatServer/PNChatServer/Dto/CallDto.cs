namespace PNChatServer.Dto
{
    public class CallDto
    {
        public int Id { get; set; }
        public string GroupCallCode { get; set; } = string.Empty;
        public string UserCode { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Created { get; set; }

        public UserDto? User { get; set; }
    }
}
