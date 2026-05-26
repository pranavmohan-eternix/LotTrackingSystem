namespace LTS.Common.Models
{
    public class Carrier
    {
        public int CarrierId { get; set; }

        public string CarrierCode { get; set; } = "";

        public string Status { get; set; } = "";

        public int Capacity { get; set; }

        public int? CurrentLocationId { get; set; }

        public string CreatedDate { get; set; } = "";

        public string CurrentLocationText =>
        CurrentLocationId.HasValue
        ? $"S-{CurrentLocationId.Value:00}"
        : "";
    }
}