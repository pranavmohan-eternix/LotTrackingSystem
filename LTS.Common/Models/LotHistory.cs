namespace LTS.Common.Models
{
    public class LotHistory
    {
        public int HistoryId { get; set; }

        public int LotId { get; set; }

        public string LotCode { get; set; } = "";

        public string Action { get; set; } = "";

        public int FromStation { get; set; }

        public int ToStation { get; set; }

        public string Status { get; set; } = "";

        public string Timestamp { get; set; } = "";

        public string LocationDisplay =>
    $"S-{ToStation:00}";
    }
}