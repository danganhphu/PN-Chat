namespace PNChatServer.Models
{
    public class Contact
    {
        public long Id { get; set; }

        public string UserCode { get; set; } = string.Empty;

        public string ContactCode { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public virtual User? UserContact { get; set; }

        public virtual User? User { get; set; }
    }
}
