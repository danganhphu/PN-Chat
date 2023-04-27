namespace PNChatServer.Models
{
    public class GroupUser
    {
        public long Id { get; set; }

        public string GroupCode { get; set; } = string.Empty;

        public string UserCode { get; set; } = string.Empty;

        public virtual Group? Group { get; set; }

        public virtual User? User { get; set; }
    }
}
