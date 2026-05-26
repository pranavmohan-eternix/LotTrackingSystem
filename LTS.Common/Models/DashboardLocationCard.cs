namespace LTS.Common.Models
{
    public class DashboardLocationCard
    {
        public string StationName { get; set; } = "";

        public string Status { get; set; } = "";

        public string CurrentLot { get; set; } = "";

        public int WaferCount { get; set; }

        public string StatusColor =>
            Status == "Available"
            ? "Green"
            : "Orange";
    }
}