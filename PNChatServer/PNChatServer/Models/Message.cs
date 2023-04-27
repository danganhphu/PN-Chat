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
        public string Type { get; set; } = string.Empty;

        public string GroupCode { get; set; } = string.Empty;

        public string? Content { get; set; }

        public string? Path { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public virtual User? UserCreatedBy { get; set; }

        public virtual Group? Group { get; set; }
    }
}
