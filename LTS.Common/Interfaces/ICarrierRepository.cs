using LTS.Common.Models;

namespace LTS.Common.Interfaces
{
    public interface ICarrierRepository
    {
        void Add(Carrier carrier);

        List<Carrier> GetAll();

        void Delete(int carrierId);

        void OccupyCarrier(
            int carrierId,
            int? stationNumber);

        void ReleaseCarrier(
            int carrierId);
    }
}