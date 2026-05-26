using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using LTS.Application.Services;
using LTS.Common.Models;
using LTS.UI.Helpers;
using LTS.UI.Views;

namespace LTS.UI.ViewModels
{
    public class LotViewModel :
        INotifyPropertyChanged
    {
        private readonly LotService _service;

        private DispatcherTimer _timer;

        public event PropertyChangedEventHandler?
            PropertyChanged;

        // LOTS

        public ObservableCollection<Lot>
            Lots
        { get; set; }

        // AVAILABLE WAFERS

        public ObservableCollection<Wafer>
            AvailableWafers
        { get; set; }

        // CARRIERS

        public ObservableCollection<Carrier>
            Carriers
        { get; set; }

        // SELECTED LOT

        private Lot? _selectedLot;

        public Lot? SelectedLot
        {
            get => _selectedLot;

            set
            {
                _selectedLot = value;

                OnPropertyChanged();
            }
        }

        // LOT CODE

        private string _lotCode = "";

        public string LotCode
        {
            get => _lotCode;

            set
            {
                _lotCode = value;

                OnPropertyChanged();
            }
        }

        // SELECTED CARRIER

        private Carrier? _selectedCarrier;

        public Carrier? SelectedCarrier
        {
            get => _selectedCarrier;

            set
            {
                _selectedCarrier = value;

                OnPropertyChanged();
            }
        }

        // AVAILABLE COUNT

        public int AvailableWaferCount =>
            AvailableWafers.Count;

        // COMMANDS

        public ICommand AddLotCommand
        { get; }

        public ICommand StartLotCommand
        { get; }

        public ICommand MoveNextCommand
        { get; }

        public ICommand DeleteLotCommand
        { get; }
        public ICommand FillWafersCommand
        { get; }

        // CONSTRUCTOR

        public LotViewModel(
            LotService service,
            List<Carrier> carriers,
            List<Wafer> wafers)
        {
            _service = service;

            // LOAD FORM DATA

            Carriers =
                new ObservableCollection<Carrier>(
                    carriers
                    .Where(x =>
                        x.Status == "Available"));

            AvailableWafers =
                new ObservableCollection<Wafer>(
                    wafers
                    .Where(x =>
                        !x.LotId.HasValue));

            // LOAD TABLE

            Lots =
                new ObservableCollection<Lot>(
                    _service.GetLots());

            // COMMANDS

            AddLotCommand =
                new RelayCommand(AddLot);

            FillWafersCommand =
                    new RelayCommand(
                        FillWafers);

            StartLotCommand =
                new RelayCommand(
                    parameter =>
                    StartLot(parameter));

            

            DeleteLotCommand =
                new RelayCommand(
                    parameter =>
                    DeleteLot(parameter));

            // AUTO REFRESH ONLY TABLE

            _timer =
                new DispatcherTimer();

            _timer.Interval =
                TimeSpan.FromSeconds(1);

            _timer.Tick +=
                (s, e) => RefreshLotGrid();

            _timer.Start();
        }

        // ADD LOT

        private void AddLot()
        {
            try
            {
                if (SelectedCarrier == null)
                {
                    MessageBox.Show(
                        "Select carrier");

                    return;
                }

                var selectedWafers =
                    AvailableWafers
                    .Where(x => x.IsSelected)
                    .ToList();

                var lot =
                    new Lot
                    {
                        LotCode =
                            LotCode,

                        CarrierId =
                            SelectedCarrier.CarrierId
                    };

                _service.AddLot(
                    lot,
                    selectedWafers);

                MessageBox.Show(
                    "Lot created successfully");

                // CLEAR FORM

                LotCode = "";

                SelectedCarrier = null;

                // FULL REFRESH ONLY AFTER CREATE

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }
        private void FillWafers()
        {
            try
            {
                if (SelectedCarrier == null)
                {
                    MessageBox.Show(
                        "Select carrier first");

                    return;
                }

                // CLEAR OLD SELECTIONS

                foreach (var wafer in AvailableWafers)
                {
                    wafer.IsSelected = false;
                }

                // GET CAPACITY

                int capacity =
                    SelectedCarrier.Capacity;

                // TAKE FIRST AVAILABLE

                var wafersToSelect =
                    AvailableWafers
                    .Take(capacity)
                    .ToList();

                // AUTO SELECT

                foreach (var wafer in wafersToSelect)
                {
                    wafer.IsSelected = true;
                }

                MessageBox.Show(
                    $"{wafersToSelect.Count} wafers selected");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        // START LOT

        private void StartLot(object? parameter)
        {
            try
            {
                if (parameter is not Lot lot)
                {
                    MessageBox.Show("Invalid lot");
                    return;
                }

                // OPEN POPUP

                var routeWindow =
                    new SelectRouteWindow();

                bool? result =
                    routeWindow.ShowDialog();

                if (result != true)
                {
                    return;
                }

                // GET SELECTED ROUTES

                var selectedStations =
                    routeWindow.ViewModel.SelectedStations;

                // VALIDATION

                if (selectedStations.Count == 0)
                {
                    MessageBox.Show(
                        "Select at least one station");

                    return;
                }

                // SAVE ROUTE

                string route =
                    string.Join(",",
                    selectedStations);

                _service.SaveRoute(
                    lot.LotId,
                    route);

                // START LOT

                _service.StartLot(
                    lot.LotId);

                MessageBox.Show(
                    "Lot started");

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }
        // DELETE LOT

        private void DeleteLot(
            object? parameter)
        {
            try
            {
                if (parameter is not Lot lot)
                {
                    MessageBox.Show(
                        "Invalid lot");

                    return;
                }

                _service.DeleteLot(
                    lot.LotId);

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        // REFRESH ONLY TABLE

        private void RefreshLotGrid()
        {
            Lots =
                new ObservableCollection<Lot>(
                    _service.GetLots());

            OnPropertyChanged(
                nameof(Lots));
        }

        // FULL REFRESH

        private void LoadData()
        {
            RefreshLotGrid();

            Carriers =
                new ObservableCollection<Carrier>(
                    _service
                    .GetAvailableCarriers());

            OnPropertyChanged(
                nameof(Carriers));

            AvailableWafers =
                new ObservableCollection<Wafer>(
                    _service
                    .GetAvailableWafers());

            OnPropertyChanged(
                nameof(AvailableWafers));

            OnPropertyChanged(
                nameof(AvailableWaferCount));
        }

        // NOTIFY

        private void OnPropertyChanged(
            [CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(name));
        }
    }
}