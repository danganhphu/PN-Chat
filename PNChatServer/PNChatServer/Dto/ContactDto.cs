namespace PNChatServer.Dto
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string UserCode { get; set; } = string.Empty;
        public string ContactCode { get; set; } = string.Empty;
        public DateTime Created { get; set; }

        public UserDto? User { get; set; }
        public UserDto? UserContact { get; set; }
    }
}
