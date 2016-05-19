using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Eurofurence.Companion.Views
{
    public sealed partial class EventsByDayPage : Page, IPageProperties
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private EventConferenceDay _currentConferenceDay;

        public string Title => _currentConferenceDay?.WeekdayFullname;
        public string Icon => "\uE163";

        public EventsByDayPage()
        {
            this.InitializeComponent();
            EventList.DataContext = null;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper => this.navigationHelper;
        public ObservableDictionary DefaultViewModel => this.defaultViewModel;

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.NavigationParameter is EventConferenceDay)
            {
                _currentConferenceDay = (e.NavigationParameter as EventConferenceDay);
                EventList.ItemsSource = _currentConferenceDay?.Entries;
            }
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void EventList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
