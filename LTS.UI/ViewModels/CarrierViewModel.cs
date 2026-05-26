using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using LTS.Application.Services;
using LTS.Common.Models;
using LTS.UI.Helpers;

namespace LTS.UI.ViewModels
{
    public class CarrierViewModel : INotifyPropertyChanged
    {
        private readonly CarrierService _service;

        private readonly DispatcherTimer _timer;

        public event PropertyChangedEventHandler?
            PropertyChanged;

        public ObservableCollection<Carrier>
            Carriers
        { get; set; }
            = new ObservableCollection<Carrier>();

        public ICommand AddCarrierCommand { get; }

        public ICommand DeleteCarrierCommand { get; }

        // INPUTS

        private string _carrierCode = "";

        public string CarrierCode
        {
            get => _carrierCode;

            set
            {
                _carrierCode = value;
                OnPropertyChanged();
            }
        }

        private int _capacity;

        public int Capacity
        {
            get => _capacity;

            set
            {
                _capacity = value;
                OnPropertyChanged();
            }
        }

        // SELECTED

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

        // CONSTRUCTOR

        public CarrierViewModel(
            CarrierService service)
        {
            _service = service;

            AddCarrierCommand =
                new RelayCommand(AddCarrier);

            DeleteCarrierCommand =
                new RelayCommand(DeleteCarrier);

            LoadCarriers();

            // AUTO REFRESH

            _timer = new DispatcherTimer();

            _timer.Interval =
                TimeSpan.FromSeconds(1);

            _timer.Tick += (s, e) =>
            {
                LoadCarriers();
            };

            _timer.Start();
        }

        // LOAD

        private void LoadCarriers()
        {
            var data =
                _service.GetCarriers();

            Carriers.Clear();

            foreach (var c in data)
            {
                Carriers.Add(c);
            }
        }

        // ADD

        private void AddCarrier()
        {
            try
            {
                var carrier = new Carrier
                {
                    CarrierCode = CarrierCode,
                    Capacity = Capacity
                };

                _service.AddCarrier(carrier);

                CarrierCode = "";
                Capacity = 0;

                LoadCarriers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // DELETE

        private void DeleteCarrier()
        {
            if (SelectedCarrier == null)
                return;

            _service.DeleteCarrier(
                SelectedCarrier.CarrierId);

            LoadCarriers();
        }

        // NOTIFY

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