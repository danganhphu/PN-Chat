namespace PNChatServer.Dto
{
    public class AccessToken
    {
        public string User { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
