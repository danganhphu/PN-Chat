namespace PNChatServer.Dto
{
    public class CallDto
    {
        public int Id { get; set; }
        public string GroupCallCode { get; set; } = null!;
        public string UserCode { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime Created { get; set; }

        public UserDto User { get; set; } = null!;
    }
}
