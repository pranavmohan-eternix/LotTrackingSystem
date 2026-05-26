using System.Windows.Controls;
using LTS.Application.Services;
using LTS.Data.Repositories;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class CarrierPage : Page
    {
        public CarrierPage()
        {
            InitializeComponent();

            var repo =
                new CarrierRepository();

            var service =
                new CarrierService(repo);

            DataContext =
                new CarrierViewModel(service);
        }
    }
}