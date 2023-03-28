namespace PNChatServer.Models
{
    public class Group
    {
        public string Code { get; set; } = null!;

        /// <summary>
        /// single: chat 1-1
        /// multi: chat 1-n
        /// </summary>
        public string Type { get; set; } = null!;

        public string? Avatar { get; set; }

        public string? Name { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime LastActive { get; set; }

        public virtual ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
