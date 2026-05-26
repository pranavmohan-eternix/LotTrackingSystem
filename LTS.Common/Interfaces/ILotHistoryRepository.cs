using LTS.Common.Models;

namespace LTS.Common.Interfaces
{
    public interface ILotHistoryRepository
    {
        void Add(LotHistory history);

        List<LotHistory> GetAll();

        List<LotHistory> GetByLot(
            int lotId);
    }
}