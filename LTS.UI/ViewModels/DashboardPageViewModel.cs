using LTS.Common.Models;
using LTS.Data.Repositories;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace LTS.UI.ViewModels
{
    public class DashboardPageViewModel :
        INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler?
            PropertyChanged;

        // SUMMARY

        public int AvailableWafers { get; set; }

        public int ActiveLots { get; set; }

        public int FreeLocations { get; set; }

        public ObservableCollection<Lot> Lots
        {
            get;
            set;
        }

        // LOCATION CARDS

        public ObservableCollection<DashboardLocationCard>
            Locations
        { get; set; }

        // RECENT LOT HISTORY

        public ObservableCollection<LotHistory>
            RecentHistory
        { get; set; }

        // RECENT LOGS

        public ObservableCollection<LogMessage>
            RecentLogs
        { get; set; }

        // CONSTRUCTOR

        public DashboardPageViewModel()
        {
            LoadDashboardData();

            var timer =
                new DispatcherTimer();

            timer.Interval =
                TimeSpan.FromSeconds(1);

            timer.Tick +=
                (s, e) => LoadDashboardData();

            timer.Start();


        }

        // LOAD DASHBOARD

        private void LoadDashboardData()
        {
            var lotRepository =
                new LotRepository();

            var waferRepository =
                new WaferRepository();

            var historyRepository =
                new LotHistoryRepository();

            var logRepository =
                new LogRepository();

            // GET DATA

            var lots =
                lotRepository.GetAll();

            var wafers =
                waferRepository.GetAll();
            Lots =
                new ObservableCollection<Lot>(
                lots);

            // SUMMARY

            AvailableWafers =
                wafers.Count(x =>
                    x.LotId == null);

            ActiveLots =
                lots.Count(x =>
                    x.Status == "InProgress");

            FreeLocations =
                10 - ActiveLots;

            // LOCATION CARDS

            Locations =
                new ObservableCollection
                <DashboardLocationCard>();

            // CREATE 10 STATIONS

            for (int i = 1; i <= 10; i++)
            {
                var runningLot =
                    lots.FirstOrDefault(x =>
                        x.CurrentStation == i &&
                        x.Status == "InProgress");

                // OCCUPIED

                if (runningLot != null)
                {
                    Locations.Add(
                        new DashboardLocationCard
                        {
                            StationName =
                                $"S-{i:00}",

                            Status =
                                "Occupied",

                            CurrentLot =
                                runningLot.LotCode,

                            WaferCount =
                                runningLot.WaferCount
                        });
                }

                // AVAILABLE

                else
                {
                    Locations.Add(
                        new DashboardLocationCard
                        {
                            StationName =
                                $"S-{i:00}",

                            Status =
                                "Available",

                            CurrentLot =
                                "-",

                            WaferCount =
                                0
                        });
                }
            }

            // TOP 5 LOT HISTORY

            RecentHistory =
                new ObservableCollection<LotHistory>(
                    historyRepository
                    .GetAll()
                    .OrderByDescending(x =>
                        x.HistoryId)
                    .Take(5));

            // TOP 5 LOGS

            RecentLogs =
                new ObservableCollection<LogMessage>(
                    logRepository
                    .GetAll()
                    .OrderByDescending(x =>
                        x.LogId)
                    .Take(5));

            // REFRESH UI

            OnPropertyChanged(nameof(AvailableWafers));

            OnPropertyChanged(nameof(ActiveLots));

            OnPropertyChanged(nameof(FreeLocations));

            OnPropertyChanged(nameof(Locations));

            OnPropertyChanged(nameof(RecentHistory));

            OnPropertyChanged(nameof(RecentLogs));
        }

        // NOTIFY UI

        private void OnPropertyChanged(
            [CallerMemberName]
            string? name = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(name));
        }
    }
}