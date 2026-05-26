using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using LTS.UI.Helpers;
using LTS.UI.Views;

namespace LTS.UI.ViewModels
{
    public class DashboardViewModel :
        INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler?
            PropertyChanged;

        // PAGE TITLE

        private string _pageTitle =
            "Dashboard";

        public string PageTitle
        {
            get => _pageTitle;

            set
            {
                _pageTitle = value;

                OnPropertyChanged();
            }
        }

        // CURRENT PAGE

        private Page? _currentPage;

        public Page? CurrentPage
        {
            get => _currentPage;

            set
            {
                _currentPage = value;

                OnPropertyChanged();
            }
        }

        // USERNAME

        private string _username;

        public string Username
        {
            get => _username;

            set
            {
                _username = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(ProfileLetter));
            }
        }

        // PROFILE LETTER

        public string ProfileLetter =>
            Username.Substring(0, 1).ToUpper();

        // COMMANDS

        public ICommand OpenDashboardCommand { get; }

        public ICommand OpenSupplierCommand { get; }

        public ICommand OpenProcessLocationCommand { get; }

        public ICommand OpenCarrierCommand { get; }

        public ICommand OpenLotCommand { get; }

        public ICommand OpenWaferCommand { get; }

        public ICommand OpenHistoryCommand { get; }

        public ICommand OpenLogCommand { get; }

        // CONSTRUCTOR

        public DashboardViewModel(
            string username)
        {
            Username = username;

            OpenDashboardCommand =
                new RelayCommand(OpenDashboard);

            OpenSupplierCommand =
                new RelayCommand(OpenSupplier);

            OpenProcessLocationCommand =
                new RelayCommand(OpenProcessLocation);

            OpenCarrierCommand =
                new RelayCommand(OpenCarrier);

            OpenLotCommand =
                new RelayCommand(OpenLot);

            OpenWaferCommand =
                new RelayCommand(OpenWafer);

            OpenHistoryCommand =
                new RelayCommand(OpenHistory);

            OpenLogCommand =
                new RelayCommand(OpenLog);

            // DEFAULT PAGE

            OpenDashboard();
        }

        // METHODS

        private void OpenDashboard()
        {
            CurrentPage =
                new DashboardPage();

            PageTitle =
                "Dashboard";
        }

        private void OpenSupplier()
        {
            CurrentPage =
                new SupplierPage();

            PageTitle =
                "Suppliers";
        }

        private void OpenProcessLocation()
        {
            CurrentPage =
                new ProcessLocationPage();

            PageTitle =
                "Process Locations";
        }

        private void OpenCarrier()
        {
            CurrentPage =
                new CarrierPage();

            PageTitle =
                "Carriers";
        }

        private void OpenLot()
        {
            CurrentPage =
                new LotPage();

            PageTitle =
                "Lots";
        }

        private void OpenWafer()
        {
            CurrentPage =
                new WaferPage();

            PageTitle =
                "Wafers";
        }

        // LOT HISTORY PAGE

        private void OpenHistory()
        {
            CurrentPage =
                new LotHistoryPage();

            PageTitle =
                "Lot History";
        }

        // LOG PAGE

        private void OpenLog()
        {
            CurrentPage =
                new LogPage();

            PageTitle =
                "Logs";
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