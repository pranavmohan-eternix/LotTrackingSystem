using System.Windows.Controls;
using LTS.Application.Services;
using LTS.Data.Repositories;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class LotPage : Page
    {
        public LotPage()
        {
            InitializeComponent();

            // REPOSITORIES

            var lotRepo =
                new LotRepository();

            var carrierRepo =
                new CarrierRepository();

            var waferRepo =
                new WaferRepository();

            var processRepo =
                new ProcessLocationRepository();

            var historyRepo =
                new LotHistoryRepository();

            var logRepo =
                new LogRepository();

            // SERVICES

            var logService =
                new LogService(logRepo);

            var lotService =
                new LotService(
                    lotRepo,
                    carrierRepo,
                    waferRepo,
                    processRepo,
                    historyRepo,
                    logService);

            // VIEWMODEL

            DataContext =
                new LotViewModel(
                    lotService,
                    carrierRepo.GetAll(),
                    waferRepo.GetAll());
        }
    }
}