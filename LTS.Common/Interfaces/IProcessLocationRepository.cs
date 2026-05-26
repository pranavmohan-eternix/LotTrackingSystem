using LTS.Common.Models;

namespace LTS.Common.Interfaces
{
    public interface IProcessLocationRepository
    {
        List<ProcessLocation> GetAll();

        void OccupyStation(
            int stationNumber,
            string lotCode,
            int waferCount);

        void ReleaseStation(
            int stationNumber);

        bool IsStationAvailable(
            int stationNumber);
        ProcessLocation? GetBySequence(
            int sequenceNo);
    }
}