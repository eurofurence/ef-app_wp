using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;
using Eurofurence.Companion.DataModel;
using Microsoft.HockeyApp.Common;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon]
    public class DealersViewModel : BindableBase
    {

        public ObservableCollection<DealerViewModel> Dealers => _dealersViewModelContext.Dealers;
        public ObservableCollection<DealerViewModel> DealerSearchResults { get; set; }

        private bool _isSearchEnabled = false;

        public bool IsSearchEnabled
        {
            get { return _isSearchEnabled;  }
            set
            {
                SetProperty(ref _isSearchEnabled, value);
                if (value == false) SearchText = string.Empty;
            }
        }

        private string _searchText = "";
        private readonly IDealersViewModelContext _dealersViewModelContext;

        public ICommand ToggleSearchEnabled { get; private set; }

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
            if (string.IsNullOrWhiteSpace(_searchText))
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
            ToggleSearchEnabled = new RelayCommand(() => IsSearchEnabled = !IsSearchEnabled);

            UpdateSearchResults();
        }
    }
}
