namespace PNChatServer.Models
{
    public class GroupUser
    {
        public long Id { get; set; }

        public string GroupCode { get; set; } = null!;

        public string UserCode { get; set; } = null!;

        public virtual Group Group { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
