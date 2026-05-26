using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using LTS.Application.Services;
using LTS.Common.Models;

namespace LTS.UI.ViewModels
{
    public class ProcessLocationViewModel :
        INotifyPropertyChanged
    {
        private readonly ProcessLocationService _service;

        private readonly DispatcherTimer _timer;

        public event PropertyChangedEventHandler?
            PropertyChanged;

        public ObservableCollection<ProcessLocation>
            Locations
        { get; set; }

        public ProcessLocationViewModel(
            ProcessLocationService service)
        {
            _service = service;

            Locations =
                new ObservableCollection<ProcessLocation>();

            LoadLocations();

            _timer =
                new DispatcherTimer();

            _timer.Interval =
                TimeSpan.FromSeconds(2);

            _timer.Tick += RefreshData;

            _timer.Start();
        }

        private void RefreshData(
            object? sender,
            EventArgs e)
        {
            LoadLocations();
        }

        private void LoadLocations()
        {
            var locations =
                _service.GetLocations();

            Locations.Clear();

            foreach (var location in locations)
            {
                Locations.Add(location);
            }
        }

        private void OnPropertyChanged(
            [CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(name));
        }
    }
}