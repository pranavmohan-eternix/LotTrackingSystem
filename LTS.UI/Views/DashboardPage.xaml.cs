using System;
using System.Windows.Controls;
using System.Windows.Threading;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class DashboardPage : Page
    {
        private readonly DispatcherTimer
            _timer;

        public DashboardPage()
        {
            InitializeComponent();

            DataContext =
                new DashboardPageViewModel();

            // AUTO REFRESH TIMER

            _timer =
                new DispatcherTimer();

            _timer.Interval =
                TimeSpan.FromSeconds(3);

            _timer.Tick += RefreshDashboard;

            _timer.Start();
        }

        // REFRESH

        private void RefreshDashboard(
            object? sender,
            EventArgs e)
        {
            DataContext =
                new DashboardPageViewModel();
        }
    }
}