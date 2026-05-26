using LTS.Common.Interfaces;
using LTS.Common.Models;
using System.Threading.Tasks;


namespace LTS.Application.Services
{
    public class LotService
    {
        private readonly ILotRepository _repo;

        private readonly ICarrierRepository _carrierRepo;

        private readonly IWaferRepository _waferRepo;

        private readonly IProcessLocationRepository
            _processRepo;

        private readonly ILotHistoryRepository
            _historyRepo;

        private readonly LogService
            _logService;

        public LotService(
            ILotRepository repo,
            ICarrierRepository carrierRepo,
            IWaferRepository waferRepo,
            IProcessLocationRepository processRepo,
            ILotHistoryRepository historyRepo,
            LogService logService)
        {
            _repo = repo;

            _carrierRepo = carrierRepo;

            _waferRepo = waferRepo;

            _processRepo = processRepo;

            _historyRepo = historyRepo;

            _logService = logService;
        }

        // ─────────────────────────────────────────
        // HELPER — get wafer ids that belong to a lot
        // ─────────────────────────────────────────

        private List<int> GetWaferIdsForLot(int lotId)
        {
            return _waferRepo
                .GetAll()
                .Where(w => w.LotId == lotId)
                .Select(w => w.WaferId)
                .ToList();
        }

        // ─────────────────────────────────────────
        // HELPER — bulk update status for all wafers in a lot
        // ─────────────────────────────────────────

        private void SetWaferStatusForLot(
            int lotId,
            string status)
        {
            foreach (var waferId in
                GetWaferIdsForLot(lotId))
            {
                _waferRepo.UpdateStatus(
                    waferId,
                    status);
            }
        }

        // ─────────────────────────────────────────
        // ADD LOT
        // Wafer status: Unallocated → Allocated
        // ─────────────────────────────────────────

        public void AddLot(
            Lot lot,
            List<Wafer> selectedWafers)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(
                    lot.LotCode))
                {
                    throw new Exception(
                        "Lot Code required");
                }

                if (selectedWafers.Count == 0)
                {
                    throw new Exception(
                        "Select wafers");
                }

                var carrier =
                    _carrierRepo
                    .GetAll()
                    .FirstOrDefault(x =>
                        x.CarrierId ==
                        lot.CarrierId);

                if (carrier == null)
                {
                    throw new Exception(
                        "Carrier not found");
                }

                if (selectedWafers.Count >
                    carrier.Capacity)
                {
                    throw new Exception(
                        "Carrier capacity exceeded");
                }

                if (carrier.Status !=
                    "Available")
                {
                    throw new Exception(
                        "Carrier already occupied");
                }

                lot.WaferCount =
                    selectedWafers.Count;

                lot.CurrentStation = 0;

                lot.Status = "Idle";

                int lotId =
                    _repo.Add(lot);

                _carrierRepo.OccupyCarrier(
                    lot.CarrierId,
                    null);

                // Assign lot + set status → Allocated
                foreach (var wafer in selectedWafers)
                {
                    _waferRepo.AssignLot(
                        wafer.WaferId,
                        lotId);
                    // AssignLot already writes 'Allocated'
                    // in the SQL — no extra call needed here
                }

                _historyRepo.Add(
                    new LotHistory
                    {
                        LotId = lotId,

                        LotCode = lot.LotCode,

                        Action = "Lot Created",

                        FromStation = 0,

                        ToStation = 0,

                        Status = "Idle",

                        Timestamp =
                            DateTime.Now.ToString(
                                "yyyy-MM-dd HH:mm:ss")
                    });

                _logService.Info(
                    $"Lot {lot.LotCode} created.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ─────────────────────────────────────────
        // GET LOTS
        // ─────────────────────────────────────────

        public List<Lot> GetLots()
        {
            return _repo.GetAll();
        }

        // ─────────────────────────────────────────
        // GET AVAILABLE CARRIERS
        // ─────────────────────────────────────────

        public List<Carrier> GetAvailableCarriers()
        {
            return _carrierRepo
                .GetAll()
                .Where(x =>
                    x.Status == "Available")
                .ToList();
        }

        // ─────────────────────────────────────────
        // GET AVAILABLE WAFERS
        // ─────────────────────────────────────────

        public List<Wafer> GetAvailableWafers()
        {
            return _waferRepo
                .GetAll()
                .Where(x =>
                    !x.LotId.HasValue)
                .ToList();
        }

        // ─────────────────────────────────────────
        // START LOT
        // Wafer status: Allocated → Processing
        //               (or Queued if station busy)
        // ─────────────────────────────────────────

        public void StartLot(int lotId)
        {
            try
            {
                var lot =
                    _repo
                    .GetAll()
                    .FirstOrDefault(x =>
                        x.LotId == lotId);

                if (lot == null)
                {
                    throw new Exception(
                        "Lot not found");
                }

                if (string.IsNullOrWhiteSpace(
                    lot.RouteStations))
                {
                    throw new Exception(
                        "No route selected");
                }

                var routeStations =
                    lot.RouteStations
                    .Split(',')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(int.Parse)
                    .ToList();

                int targetStation =
                    routeStations.First();

                bool available =
                    _processRepo
                    .IsStationAvailable(
                        targetStation);

                if (!available)
                {
                    _repo.UpdateStatus(
                        lotId,
                        "Queued");

                    // Wafer status → Queued
                    SetWaferStatusForLot(lotId, "Queued");

                    _logService.Warn(
                        $"Lot {lot.LotCode} queued.");

                    return;
                }

                _processRepo.OccupyStation(
                    targetStation,
                    lot.LotCode,
                    lot.WaferCount);

                _repo.MoveNext(
                    lotId,
                    targetStation,
                    "InProgress");

                // Wafer status → Processing
                SetWaferStatusForLot(lotId, "Processing");

                _carrierRepo.OccupyCarrier(
                    lot.CarrierId,
                    targetStation);

                _historyRepo.Add(
                    new LotHistory
                    {
                        LotId = lot.LotId,

                        LotCode = lot.LotCode,

                        Action = "Started",

                        FromStation =
                            targetStation == 1
                            ? 0
                            : targetStation,

                        ToStation =
                            targetStation,

                        Status = "InProgress",

                        Timestamp =
                            DateTime.Now.ToString(
                                "yyyy-MM-dd HH:mm:ss")
                    });

                _logService.Info(
                    $"Lot {lot.LotCode} started at S-{targetStation:00}.");

                _ = AutoMoveLot(lotId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ─────────────────────────────────────────
        // AUTO MOVE
        // ─────────────────────────────────────────

        private async Task AutoMoveLot(int lotId)
        {
            while (true)
            {
                await Task.Delay(5000);

                var lot =
                    _repo
                    .GetAll()
                    .FirstOrDefault(x =>
                        x.LotId == lotId);

                if (lot == null)
                {
                    break;
                }

                if (lot.Status == "Completed")
                {
                    break;
                }

                if (lot.Status == "Queued")
                {
                    continue;
                }

                MoveNext(lotId);
            }
        }

        // ─────────────────────────────────────────
        // MOVE NEXT
        // Wafer status stays Processing while moving
        //             → Queued if next station busy
        // ─────────────────────────────────────────

        public void MoveNext(int lotId)
        {
            try
            {
                var lot =
                    _repo
                    .GetAll()
                    .FirstOrDefault(x =>
                        x.LotId == lotId);

                if (lot == null)
                {
                    throw new Exception(
                        "Lot not found");
                }

                if (lot.CurrentStation == 0)
                {
                    throw new Exception(
                        "Start the lot first");
                }

                if (lot.Status == "Completed")
                {
                    return;
                }

                int currentStation =
                    lot.CurrentStation;

                var routeStations =
                    lot.RouteStations?
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();

                if (routeStations == null ||
                    routeStations.Count == 0)
                {
                    throw new Exception(
                        "No route selected");
                }

                int currentIndex =
                    routeStations.IndexOf(
                        currentStation);

                if (currentIndex + 1 >=
                    routeStations.Count)
                {
                    CompleteLot(lot);

                    return;
                }

                int nextStation =
                    routeStations[
                        currentIndex + 1];

                bool available =
                    _processRepo
                    .IsStationAvailable(
                        nextStation);

                if (!available)
                {
                    _repo.UpdateStatus(
                        lotId,
                        "Queued");

                    // Wafer status → Queued
                    SetWaferStatusForLot(lotId, "Queued");

                    _logService.Warn(
                        $"Lot {lot.LotCode} queued waiting for S-{nextStation:00}.");

                    return;
                }

                _processRepo.ReleaseStation(
                    currentStation);

                _processRepo.OccupyStation(
                    nextStation,
                    lot.LotCode,
                    lot.WaferCount);

                _repo.MoveNext(
                    lotId,
                    nextStation,
                    "InProgress");

                // Wafer status stays Processing
                SetWaferStatusForLot(lotId, "Processing");

                _carrierRepo.OccupyCarrier(
                    lot.CarrierId,
                    nextStation);

                _historyRepo.Add(
                    new LotHistory
                    {
                        LotId = lot.LotId,

                        LotCode = lot.LotCode,

                        Action = "Moved",

                        FromStation = currentStation,

                        ToStation = nextStation,

                        Status = "InProgress",

                        Timestamp =
                            DateTime.Now.ToString(
                                "yyyy-MM-dd HH:mm:ss")
                    });

                _logService.Info(
                    $"Lot {lot.LotCode} moved from S-{currentStation:00} to S-{nextStation:00}.");

                ProcessQueuedLots();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ─────────────────────────────────────────
        // COMPLETE LOT
        // Wafer status: Processing → Completed
        // ─────────────────────────────────────────

        private void CompleteLot(Lot lot)
        {
            _repo.UpdateStatus(
                    lot.LotId,
                    "Completed");

            _repo.MoveNext(
                lot.LotId,
                lot.CurrentStation,
                "Completed");

            // Wafer status → Completed
            SetWaferStatusForLot(lot.LotId, "Completed");

            _processRepo.ReleaseStation(
                        lot.CurrentStation);

            _carrierRepo.ReleaseCarrier(
                lot.CarrierId);

            _historyRepo.Add(
                new LotHistory
                {
                    LotId = lot.LotId,

                    LotCode = lot.LotCode,

                    Action = "Completed",

                    FromStation = lot.CurrentStation,

                    ToStation = lot.CurrentStation,

                    Status = "Completed",

                    Timestamp =
                        DateTime.Now.ToString(
                            "yyyy-MM-dd HH:mm:ss")
                });

            _logService.Info(
                $"Lot {lot.LotCode} completed.");

            ProcessQueuedLots();
        }

        // ─────────────────────────────────────────
        // PROCESS QUEUED LOTS
        // Wafer status: Queued → Processing (when slot opens)
        // ─────────────────────────────────────────

        private void ProcessQueuedLots()
        {
            var queuedLots =
                _repo
                .GetAll()
                .Where(x => x.Status == "Queued")
                .ToList();

            foreach (var lot in queuedLots)
            {
                var routeStations =
                    lot.RouteStations
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();

                int nextStation = 0;

                if (lot.CurrentStation == 0)
                {
                    nextStation = routeStations.First();
                }
                else
                {
                    int currentIndex =
                        routeStations.IndexOf(
                            lot.CurrentStation);

                    if (currentIndex + 1 < routeStations.Count)
                    {
                        nextStation =
                            routeStations[currentIndex + 1];
                    }
                    else
                    {
                        continue;
                    }
                }

                bool available =
                    _processRepo
                    .IsStationAvailable(nextStation);

                if (!available)
                {
                    continue;
                }

                if (lot.CurrentStation != 0)
                {
                    _processRepo.ReleaseStation(
                        lot.CurrentStation);
                }

                _processRepo.OccupyStation(
                    nextStation,
                    lot.LotCode,
                    lot.WaferCount);

                _repo.MoveNext(
                    lot.LotId,
                    nextStation,
                    "InProgress");

                // Wafer status → Processing (slot opened)
                SetWaferStatusForLot(lot.LotId, "Processing");

                _carrierRepo.OccupyCarrier(
                    lot.CarrierId,
                    nextStation);

                _historyRepo.Add(
                    new LotHistory
                    {
                        LotId = lot.LotId,
                        LotCode = lot.LotCode,
                        Action = "Moved From Queue",
                        FromStation = lot.CurrentStation,
                        ToStation = nextStation,
                        Status = "InProgress",
                        Timestamp =
                            DateTime.Now.ToString(
                                "yyyy-MM-dd HH:mm:ss")
                    });

                _logService.Info(
                    $"Queued lot {lot.LotCode} moved to S-{nextStation:00}.");
            }
        }

        // ─────────────────────────────────────────
        // DELETE LOT
        // Wafer status: any → Unallocated, LotId cleared
        // ─────────────────────────────────────────

        public void DeleteLot(int lotId)
        {
            var lot =
                _repo
                .GetAll()
                .FirstOrDefault(x =>
                    x.LotId == lotId);

            if (lot == null)
            {
                throw new Exception(
                    "Lot not found");
            }

            if (lot.Status != "Idle")
            {
                throw new Exception(
                    "Only Idle lots can be deleted");
            }

            // Unassign all wafers → Unallocated + LotId = null
            foreach (var waferId in
                GetWaferIdsForLot(lotId))
            {
                _waferRepo.UnassignLot(waferId);
            }

            _repo.Delete(lotId);

            _logService.Info(
                $"Lot {lot.LotCode} deleted.");
        }

        // ─────────────────────────────────────────
        // SAVE ROUTE
        // ─────────────────────────────────────────

        public void SaveRoute(
            int lotId,
            string routeStations)
        {
            _repo.UpdateRouteStations(
                lotId,
                routeStations);
        }
    }
}
