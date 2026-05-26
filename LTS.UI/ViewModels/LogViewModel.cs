using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LTS.Application.Services;
using LTS.Common.Models;

namespace LTS.UI.ViewModels
{
    public class LogViewModel :
        INotifyPropertyChanged
    {
        private readonly LogService
            _logService;

        public event PropertyChangedEventHandler?
            PropertyChanged;

        // LOGS

        public ObservableCollection<LogMessage>
            Logs
        { get; set; }

        // CONSTRUCTOR

        public LogViewModel(
            LogService logService)
        {
            _logService =
                logService;

            Logs =
                new ObservableCollection<LogMessage>(
                    _logService.GetLogs());
        }

        // REFRESH

        public void Refresh()
        {
            Logs =
                new ObservableCollection<LogMessage>(
                    _logService.GetLogs());

            OnPropertyChanged(nameof(Logs));
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