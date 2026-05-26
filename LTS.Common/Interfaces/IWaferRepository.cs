using LTS.Common.Models;

namespace LTS.Common.Interfaces
{
    public interface IWaferRepository
    {
        // ══════════════════════════════════════
        // CREATE
        // ══════════════════════════════════════

        void Add(Wafer wafer);

        // ══════════════════════════════════════
        // READ
        // ══════════════════════════════════════

        List<Wafer> GetAll();

        // CHECK DUPLICATE SERIAL

        bool ExistsBySerial(
            string serialNo);

        // ══════════════════════════════════════
        // DELETE
        // ══════════════════════════════════════

        void Delete(int waferId);

        // ══════════════════════════════════════
        // LOT ASSIGNMENT
        // ══════════════════════════════════════

        void AssignLot(
            int waferId,
            int lotId);

        // Clears LotId and resets status

        void UnassignLot(
            int waferId);

        // ══════════════════════════════════════
        // STATUS
        // ══════════════════════════════════════

        void UpdateStatus(
            int waferId,
            string status);
    }
}