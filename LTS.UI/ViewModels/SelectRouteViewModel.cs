using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LTS.Common.Models;
using LTS.UI.Helpers;

namespace LTS.UI.ViewModels
{
    public class SelectRouteViewModel :
        INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler?
            PropertyChanged;

        // SELECTED ROUTES

        public List<int> SelectedStations
        {
            get;
            private set;
        } = new();

        // STATIONS

        public ObservableCollection<RouteStation>
            Stations
        { get; set; }

        // COMMAND

        public ICommand StartProcessCommand
        { get; }

        // WINDOW CLOSE ACTION

        public Action? CloseAction
        { get; set; }

        // CONSTRUCTOR

        public SelectRouteViewModel()
        {
            Stations =
                new ObservableCollection<RouteStation>();

            // CREATE 10 STATIONS

            for (int i = 1; i <= 10; i++)
            {
                Stations.Add(
                    new RouteStation
                    {
                        StationNumber = i,

                        StationName =
                            $"S-{i:00}"
                    });
            }

            StartProcessCommand =
                new RelayCommand(
                    StartProcess);
        }

        // START PROCESS

        private void StartProcess()
        {
            SelectedStations =
                Stations
                .Where(x => x.IsSelected)
                .Select(x => x.StationNumber)
                .ToList();

            CloseAction?.Invoke();
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