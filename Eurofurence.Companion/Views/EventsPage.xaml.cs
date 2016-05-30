using Ninject;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DependencyResolution;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.ViewModel;

namespace Eurofurence.Companion.Views
{

    public sealed partial class EventsPage : Page, IPageProperties, ISearchInteraction

    {
        private const string STATE_EVENTPIVOT_INDEX = "eventPivot.Index";
        private const string STATE_SEARCHTEXT = "searchText";

        private EventsViewModel ViewModel => (EventsViewModel)DataContext;

        public EventsPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }


        public NavigationHelper NavigationHelper { get; }

        public const string PAGE_ICON = "\uE163";


        public string Title => Translations.EventSchedule_Title;
        public string Icon => PAGE_ICON;

        public string PlaceholderText => "Search for events";
        public string DefaultSearchText => "";

        public bool IsSearchAvailable => true;

        public SearchBarViewModel SearchBarViewModel { get; set; }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            SearchBarViewModel.IsSearchAvailable = true;
            SearchBarViewModel.PropertyChanged += (s, a) =>
            {
                switch (a.PropertyName)
                {
                    case nameof(SearchBarViewModel.SearchText):
                        ViewModel.SearchText = SearchBarViewModel.SearchText;
                        break;
                }

                FlipViewMain.SelectedItem = SearchBarViewModel.IsSearchExpanded && !string.IsNullOrEmpty(ViewModel.SearchText) ? 
                    FlipViewItemSearch : FlipViewItemEventPivot;
            };


            var previousSearchText = (string)(e.PageState?[STATE_SEARCHTEXT] ?? "");
            if (!string.IsNullOrWhiteSpace(previousSearchText))
            {
                SearchBarViewModel.IsSearchExpanded = true;
                SearchBarViewModel.SearchText = previousSearchText;
            }
        }


        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {

            e.PageState.Add(STATE_SEARCHTEXT, ViewModel.SearchText);
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        
        private void ViewByDay_ItemClick(object sender, ItemClickEventArgs e)
        {
            KernelResolver.Current.Get<ViewModelLocator>()
                .NavigationViewModel.NavigateToEventsByDayPage.Execute(e.ClickedItem);
        }

    }
}
