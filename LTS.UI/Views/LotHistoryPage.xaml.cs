using System.Windows.Controls;
using LTS.Data.Repositories;
using LTS.UI.ViewModels;

namespace LTS.UI.Views
{
    public partial class LotHistoryPage : Page
    {
        public LotHistoryPage()
        {
            InitializeComponent();

            // REPOSITORY

            var repo =
                new LotHistoryRepository();

            // VIEWMODEL

            DataContext =
                new LotHistoryViewModel(
                    repo);
        }
    }
}