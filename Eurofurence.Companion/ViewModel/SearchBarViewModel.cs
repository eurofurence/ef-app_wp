using Eurofurence.Companion.Common;

namespace Eurofurence.Companion.ViewModel
{
    public class SearchBarViewModel : BindableBase
    {
        private string _placeholderText;
        private string _searchText;
        private bool _isSearchAvailable;
        private bool _isSearchExpanded;

        public RelayCommand ToggleSearchBarExpansion { get; private set; }

        public SearchBarViewModel()
        {
            InitializeDispatcherFromCurrentThread();
            ToggleSearchBarExpansion = new RelayCommand(_ => {
                IsSearchExpanded = !IsSearchExpanded; 
            });
        }

        public string PlaceholderText
        {
            get { return _placeholderText; }
            set { SetProperty(ref _placeholderText, value); }
        }

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public bool IsSearchAvailable
        {
            get { return _isSearchAvailable; }
            set { SetProperty(ref _isSearchAvailable, value); }
        }
        public bool IsSearchExpanded
        {
            get { return _isSearchExpanded; }
            set { SetProperty(ref _isSearchExpanded, value); }
        }
    }
}
