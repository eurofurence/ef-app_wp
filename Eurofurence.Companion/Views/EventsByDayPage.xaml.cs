using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.ViewModel.Local;
using Eurofurence.Companion.ViewModel.Local.Entity;

namespace Eurofurence.Companion.Views
{
    public sealed partial class EventsByDayPage : Page, IPagePropertiesExtended
    {
        private EventsViewModel _typedViewModel => (EventsViewModel) DataContext;

        //public string Title => _currentViewModel?.Entity.WeekdayFullname;
        //public string Icon => "\uE163";

        public EventsByDayPage()
        {
            InitializeComponent();
            //EventList.DataContext = null;


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
                SelectConferenceDay((EventConferenceDayViewModel) e.NavigationParameter);
            }
            else
            {
                NavigationHelper.GoBack();
            }
        }

        private void SelectConferenceDay(EventConferenceDayViewModel conferenceDay)
        {
            foreach (var day in _typedViewModel.EventConferenceDays)
            {
                day.IsSelected = day == conferenceDay;
            }
            TitleChanged?.Invoke(this, conferenceDay.Entity.Name);
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

        private bool _isLoaded = false;

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoaded) return;
            SelectConferenceDay((EventConferenceDayViewModel) E_FlipView_Events.SelectedItem);
        }

        public event EventHandler<string> TitleChanged;

        private async void E_FlipView_Events_Loaded(object sender, RoutedEventArgs e)
        {
            E_FlipView_Events.SelectedIndex = -1;
            E_FlipView_Events.SelectedItem =
                _typedViewModel.EventConferenceDays.FirstOrDefault(a => a.IsSelected);
            _isLoaded = true;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            SelectConferenceDay((EventConferenceDayViewModel)e.ClickedItem);
            E_FlipView_Events.SelectedItem =
                _typedViewModel.EventConferenceDays.FirstOrDefault(a => a.IsSelected);
        }
    }
}
