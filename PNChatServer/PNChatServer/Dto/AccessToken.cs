namespace PNChatServer.Dto
{
    public class AccessToken
    {
        public string User { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Avatar { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
