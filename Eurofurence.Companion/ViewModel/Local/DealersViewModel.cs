using System;
using System.Collections.ObjectModel;
using System.Linq;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon]
    public class DealersViewModel : BindableBase
    {

        public ObservableCollection<DealerViewModel> Dealers => _dealersViewModelContext.Dealers;
        public ObservableCollection<DealerViewModel> DealerSearchResults { get; set; }

        private string _searchText = "";
        private readonly IDealersViewModelContext _dealersViewModelContext;

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

            foreach (var result in Dealers.Where(e => e.Entity.DisplayName.ToLower().Contains(_searchText.ToLower())))
            {
                DealerSearchResults.Add(result);
            }
        }

        public DealersViewModel(IDealersViewModelContext dealersViewModelContext)
        {
            _dealersViewModelContext = dealersViewModelContext;

            DealerSearchResults = new ObservableCollection<DealerViewModel>();
        }
    }
}
