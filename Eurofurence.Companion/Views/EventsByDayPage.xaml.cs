using Eurofurence.Companion.Common;
using Eurofurence.Companion.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.ViewModel.Local;


namespace Eurofurence.Companion.Views
{
    public sealed partial class EventsByDayPage : Page, IPageProperties
    {
        private EventConferenceDayViewModel _currentConferenceDay;

        public string Title => _currentConferenceDay?.Entity.WeekdayFullname;
        public string Icon => "\uE163";

        public EventsByDayPage()
        {
            InitializeComponent();
            EventList.DataContext = null;

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper { get; }

        public ObservableDictionary DefaultViewModel { get; } = new ObservableDictionary();

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.NavigationParameter is EventConferenceDayViewModel)
            {
                _currentConferenceDay = e.NavigationParameter as EventConferenceDayViewModel;
                EventList.ItemsSource = _currentConferenceDay?.EventEntries;
            }
            else
            {
                NavigationHelper.GoBack();
            }
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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

        private void EventList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
