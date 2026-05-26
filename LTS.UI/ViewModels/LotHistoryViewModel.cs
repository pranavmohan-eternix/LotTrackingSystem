using System.Collections.ObjectModel;
using LTS.Common.Interfaces;
using LTS.Common.Models;

namespace LTS.UI.ViewModels
{
    public class LotHistoryViewModel
    {
        public ObservableCollection<LotHistory>
            HistoryList
        { get; set; }

        public LotHistoryViewModel(
            ILotHistoryRepository repo)
        {
            HistoryList =
                new ObservableCollection<LotHistory>(
                    repo.GetAll());
        }
    }
}