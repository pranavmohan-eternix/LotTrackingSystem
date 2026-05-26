using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.Application.Services
{
    public class WaferService
    {
        private readonly IWaferRepository _repo;

        public WaferService(
            IWaferRepository repo)
        {
            _repo = repo;
        }

        // ══════════════════════════════════════
        // ADD SINGLE WAFER
        // ══════════════════════════════════════

        public void AddWafer(Wafer wafer)
        {
            if (string.IsNullOrWhiteSpace(
                wafer.WaferSerialNo))
            {
                throw new Exception(
                    "Wafer Serial No required");
            }

            // CHECK DUPLICATE

            if (_repo.ExistsBySerial(
        wafer.WaferSerialNo))
            {
                throw new Exception(
                    "Wafer Serial already exists");
            }
        
            wafer.WaferStatus =
                "Unallocated";

            wafer.CreatedOn =
                DateTime.Now
                .ToString("dd-MM-yyyy");

            _repo.Add(wafer);
        }

        // ══════════════════════════════════════
        // GENERATE BULK WAFERS
        // ══════════════════════════════════════

        public int GenerateWafers(
            string prefix,
            int count,
            int supplierId)
        {
            int createdCount = 0;

            for (int i = 1;
                 i <= count;
                 i++)
            {
                string serial =
                    $"{prefix}-{i:D3}";

                // SKIP DUPLICATES

                bool exists =
                    _repo.ExistsBySerial(
                        serial);

                if (exists)
                {
                    continue;
                }

                var wafer =
                    new Wafer
                    {
                        WaferSerialNo =
                            serial,

                        SupplierId =
                            supplierId,

                        WaferStatus =
                            "Unallocated",

                        CreatedOn =
                            DateTime.Now
                            .ToString("dd-MM-yyyy")
                    };

                _repo.Add(wafer);

                createdCount++;
            }

            return createdCount;
        }

        // ══════════════════════════════════════
        // GET ALL
        // ══════════════════════════════════════

        public List<Wafer> GetWafers()
        {
            return _repo.GetAll();
        }

        // ══════════════════════════════════════
        // DELETE
        // ══════════════════════════════════════

        public void DeleteWafer(int waferId)
        {
            var wafer =
                _repo.GetAll()
                .FirstOrDefault(x =>
                    x.WaferId == waferId);

            if (wafer == null)
            {
                throw new Exception(
                    "Wafer not found");
            }

            // DO NOT DELETE ALLOCATED

            if (wafer.LotId.HasValue)
            {
                throw new Exception(
                    "Allocated wafer cannot be deleted");
            }

            _repo.Delete(waferId);
        }
    }
}