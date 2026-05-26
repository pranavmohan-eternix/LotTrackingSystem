namespace LTS.Common.Models
{
    public class Wafer
    {
        public int WaferId { get; set; }

        public string WaferSerialNo { get; set; } = "";

        public int SupplierId { get; set; }

        public int? LotId { get; set; }

        public string WaferStatus { get; set; } = "";

        public string CreatedOn { get; set; } = "";

        // DISPLAY VALUE

        public string SupplierName { get; set; } = "";

        public bool IsSelected { get; set; }
    }
}