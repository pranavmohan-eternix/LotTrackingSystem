namespace LTS.Common.Models
{
    public class ProcessLocation
    {
        public int ProcessLocationId { get; set; }

        public string StationName { get; set; } = "";

        public int SequenceNo { get; set; }

        public string Status { get; set; } = "";

        public string CurrentLot { get; set; } = "";

        public int WaferCount { get; set; }
    }
}