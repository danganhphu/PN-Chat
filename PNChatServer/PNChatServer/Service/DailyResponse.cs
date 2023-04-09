namespace PNChatServer.Service
{
    public class DailyResponse
    {
        public string id { get; set; } = null!;
        public string name { get; set; } = null!;
        public string api_created { get; set; } = null!;
        public string url { get; set; } = null!;
        public DateTime created_at { get; set; }
    }
}
