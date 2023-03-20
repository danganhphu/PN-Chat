namespace PNChatServer.Models
{
    public class Contact
    {
        public long Id { get; set; }

        public string UserCode { get; set; } = null!;

        public string ContactCode { get; set; } = null!;

        public DateTime Created { get; set; }

        public virtual User UserContact { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
