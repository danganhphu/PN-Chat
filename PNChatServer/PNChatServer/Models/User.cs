namespace PNChatServer.Models
{
    public class User
    {
        public string Code { get; set; } = null!;

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public string? FullName { get; set; }

        public string? Dob { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Avatar { get; set; }

        public string? Gender { get; set; }

        public DateTime? LastLogin { get; set; }

        public string? CurrentSession { get; set; }

        public virtual ICollection<Call> Calls { get; } = new List<Call>();

        public virtual ICollection<Contact> Contacts { get; } = new List<Contact>();

        public virtual ICollection<Contact> ContactUsers { get; } = new List<Contact>();

        public virtual ICollection<GroupUser> GroupUsers { get; } = new List<GroupUser>();

        public virtual ICollection<Message> Messages { get; } = new List<Message>();
    }
}
