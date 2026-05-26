using System.Windows.Controls;
using LTS.Application.Services;
using LTS.Data.Repositories;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class LogPage : Page
    {
        public LogPage()
        {
            InitializeComponent();

            // REPOSITORY

            var logRepo =
                new LogRepository();

            // SERVICE

            var logService =
                new LogService(logRepo);

            // VIEWMODEL

            DataContext =
                new LogViewModel(
                    logService);
        }
    }
}