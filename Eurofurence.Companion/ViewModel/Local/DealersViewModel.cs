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
        private const string ALL_CATEGORIES = "All Categories";

        public ObservableCollection<DealerViewModel> Dealers => _dealersViewModelContext.Dealers;
        public ObservableCollection<DealerViewModel> DealerSearchResults { get; set; }

        public ObservableCollection<string> Categories { get; set; }

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
        private string _searchCategory = ALL_CATEGORIES;
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

        public string SearchCategory
        {
            get { return _searchCategory; }
            set
            {
                SetProperty(ref _searchCategory, value);
                UpdateSearchResults();
            }
        }

        private void UpdateSearchResults()
        {
            DealerSearchResults.Clear();

            var results = Dealers.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(_searchText))
            {
                results = results.Where(e => e.Entity.DisplayName.ToLower().Contains(_searchText.ToLower()));
            }

            if (_searchCategory != ALL_CATEGORIES)
            {
                results = results.Where(e => e.Entity.Categories.Contains(_searchCategory));
            }


            foreach (var result in results)
            {
                DealerSearchResults.Add(result);
            }
        }

        public DealersViewModel(IDealersViewModelContext dealersViewModelContext)
        {
            _dealersViewModelContext = dealersViewModelContext;

            DealerSearchResults = new ObservableCollection<DealerViewModel>();
            Categories = new ObservableCollection<string>();
            ToggleSearchEnabled = new RelayCommand(() => IsSearchEnabled = !IsSearchEnabled);

            CalculateCategories();
            UpdateSearchResults();
        }

        private void CalculateCategories()
        {
            Categories.Clear();
            Categories.Add(ALL_CATEGORIES);


            var distinctCategories = _dealersViewModelContext
                .Dealers
                .SelectMany(a => a.Categories)
                .Distinct()
                .OrderBy(a => a);

            foreach (var category in distinctCategories)
                Categories.Add(category);
        }
    }
}
