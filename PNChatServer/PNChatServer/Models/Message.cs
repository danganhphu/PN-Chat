namespace PNChatServer.Models
{
    public class Message
    {
        public long Id { get; set; }

        /// <summary>
        /// text
        /// media
        /// attachment
        /// </summary>
        public string Type { get; set; } = null!;

        public string GroupCode { get; set; } = null!;

        public string? Content { get; set; }

        public string? Path { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = null!;

        public virtual User UserCreatedBy { get; set; } = null!;

        public virtual Group Group { get; set; } = null!;
    }
}
