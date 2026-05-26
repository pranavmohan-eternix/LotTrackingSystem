using LTS.Common.Models;

namespace LTS.Common.Interfaces
{
    public interface ILotRepository
    {
        int Add(Lot lot);

        List<Lot> GetAll();

        void StartLot(int lotId);

        void Delete(int lotId);

        void MoveNext(
                    int lotId,
                    int nextStation,
                    string status);
        void UpdateStatus(
                    int lotId,
                    string status);
        void UpdateRouteStations(
                    int lotId,
                    string routeStations);
    }
}