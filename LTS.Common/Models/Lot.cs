namespace LTS.Common.Models
{
    public class Lot
    {
        public int LotId { get; set; }

        public string LotCode { get; set; } = "";


        public int CarrierId { get; set; }

        public int WaferCount { get; set; }

        public int CurrentStation { get; set; }



        public string Status { get; set; } = "";

        // DISPLAY VALUES


        public string CarrierCode { get; set; } = "";
        public string RouteStations { get; set; }
    }
}