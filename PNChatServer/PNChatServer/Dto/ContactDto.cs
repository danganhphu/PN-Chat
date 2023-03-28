namespace PNChatServer.Dto
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string UserCode { get; set; } = null!;
        public string ContactCode { get; set; } = null!;
        public DateTime Created { get; set; }

        public UserDto User { get; set; } = null!;
        public UserDto UserContact { get; set; } = null!;
    }
}
