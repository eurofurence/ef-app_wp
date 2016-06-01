using System;
using System.Collections.ObjectModel;
using System.Linq;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon]
    public class DealersViewModel : BindableBase
    {
        private readonly IDataContext _dataContext;

        public ObservableCollection<Dealer> Dealers => _dataContext.Dealers;

        public ObservableCollection<Dealer> DealerSearchResults { get; set; }

        private string _searchText = "";
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                UpdateSearchResults();
            }
        }

        private void UpdateSearchResults()
        {
            DealerSearchResults.Clear();
            if (String.IsNullOrWhiteSpace(_searchText))
            {
                foreach(var dealer in Dealers)
                {
                    DealerSearchResults.Add(dealer);
                }
                return;
            }

            foreach (var result in Dealers.Where(e => e.DisplayName.ToLower().Contains(_searchText.ToLower())))
            {
                DealerSearchResults.Add(result);
            }
        }

        public DealersViewModel(IDataContext dataContext)
        {
            _dataContext = dataContext;
            DealerSearchResults = new ObservableCollection<Dealer>();

            //if (DesignMode.DesignModeEnabled) SearchText = "";
        }
    }
}
