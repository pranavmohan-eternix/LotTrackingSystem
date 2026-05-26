using System.Windows;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class SelectRouteWindow : Window
    {
        public SelectRouteViewModel ViewModel
        { get; }

        public SelectRouteWindow()
        {
            InitializeComponent();

            ViewModel =
                new SelectRouteViewModel();

            DataContext = ViewModel;

            ViewModel.CloseAction = () =>
            {
                DialogResult = true;

                Close();
            };
        }
    }
}