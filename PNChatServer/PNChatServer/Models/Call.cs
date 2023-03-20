namespace PNChatServer.Models
{
    public class Call
    {
        public int Id { get; set; }

        public string GroupCallCode { get; set; } = null!;

        public string UserCode { get; set; } = null!;

        public string Url { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime Created { get; set; }

        public virtual GroupCall GroupCall { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
