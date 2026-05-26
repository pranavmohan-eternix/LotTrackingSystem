using System.Windows.Controls;
using LTS.Application.Services;
using LTS.Data.Repositories;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class WaferPage : Page
    {
        public WaferPage()
        {
            InitializeComponent();

            // REPOSITORIES

            var waferRepo =
                new WaferRepository();

            var supplierRepo =
                new SupplierRepository();

            // SERVICE

            var service =
                new WaferService(
                    waferRepo);

            // VIEWMODEL

            DataContext =
                new WaferViewModel(
                    service,
                    supplierRepo.GetAll());
        }
    }
}