﻿namespace PNChatServer.Models
{
    public class Call
    {
        public int Id { get; set; }

        public string GroupCallCode { get; set; }

        public string UserCode { get; set; }

        public string Url { get; set; }

        public string Status { get; set; }

        public DateTime Created { get; set; }

        public virtual GroupCall GroupCall { get; set; }

        public virtual User User { get; set; }
    }
}
