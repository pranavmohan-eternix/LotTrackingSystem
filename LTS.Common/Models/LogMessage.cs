namespace LTS.Common.Models
{
    public class LogMessage
    {
        public int LogId { get; set; }

        public string Level { get; set; } = "";

        public string Message { get; set; } = "";

        public string Timestamp { get; set; } = "";
    }
}