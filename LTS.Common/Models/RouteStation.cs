using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LTS.Common.Models
{
    public class RouteStation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler?
            PropertyChanged;

        public int StationNumber
        { get; set; }

        public string StationName
        { get; set; } = "";

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;

            set
            {
                _isSelected = value;

                OnPropertyChanged();
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