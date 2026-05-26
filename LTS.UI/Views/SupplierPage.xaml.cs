using System.Windows.Controls;
using LTS.Application.Services;
using LTS.Data.Repositories;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class SupplierPage : Page
    {
        public SupplierPage()
        {
            InitializeComponent();

            var repo = new SupplierRepository();

            var service = new SupplierService(repo);

            DataContext =
                new SupplierViewModel(service);
        }
    }
}