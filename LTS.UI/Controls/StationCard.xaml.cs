using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace LTS.UI.Controls
{
    public partial class StationCard : UserControl
    {
        // ══════════════════════════════════════
        //  DEPENDENCY PROPERTIES
        // ══════════════════════════════════════

        public static readonly DependencyProperty StationNameProperty =
            DependencyProperty.Register(
                nameof(StationName),
                typeof(string),
                typeof(StationCard),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register(
                nameof(Status),
                typeof(string),
                typeof(StationCard),
                new PropertyMetadata(string.Empty, OnStatusChanged));

        public static readonly DependencyProperty CurrentLotProperty =
            DependencyProperty.Register(
                nameof(CurrentLot),
                typeof(string),
                typeof(StationCard),
                new PropertyMetadata("—"));

        public static readonly DependencyProperty WaferCountProperty =
            DependencyProperty.Register(
                nameof(WaferCount),
                typeof(int),
                typeof(StationCard),
                new PropertyMetadata(0));

        // ══════════════════════════════════════
        //  CLR WRAPPERS
        // ══════════════════════════════════════

        public string StationName
        {
            get => (string)GetValue(StationNameProperty);
            set => SetValue(StationNameProperty, value);
        }

        public string Status
        {
            get => (string)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public string CurrentLot
        {
            get => (string)GetValue(CurrentLotProperty);
            set => SetValue(CurrentLotProperty, value);
        }

        public int WaferCount
        {
            get => (int)GetValue(WaferCountProperty);
            set => SetValue(WaferCountProperty, value);
        }

        // ══════════════════════════════════════
        //  CONSTRUCTOR
        // ══════════════════════════════════════

        public StationCard()
        {
            InitializeComponent();
        }

        // ══════════════════════════════════════
        //  STATUS CHANGED → SWAP + ANIMATE
        // ══════════════════════════════════════

        private static void OnStatusChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is StationCard card)
                card.ApplyStatus((string)e.NewValue);
        }

        private void ApplyStatus(string status)
        {
            // Stop occupied pulse if running
            var occPulse = (Storyboard)Resources["OccupiedPulse"];
            occPulse.Stop(this);

            if (status == "Occupied")
            {
                AvailableCard.Visibility = Visibility.Collapsed;
                OccupiedCard.Visibility = Visibility.Visible;
                occPulse.Begin(this, true);
            }
            else
            {
                // Available (default)
                OccupiedCard.Visibility = Visibility.Collapsed;
                AvailableCard.Visibility = Visibility.Visible;
            }
        }
    }
}
