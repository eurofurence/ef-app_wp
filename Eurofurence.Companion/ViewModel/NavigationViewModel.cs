using Eurofurence.Companion.Common;

namespace Eurofurence.Companion.ViewModel
{
    public class NavigationViewModel : BindableBase
    {
        private ILayoutPage _navigationProvider;

        public RelayCommand NavigateToMainPage { get; set; }
        public RelayCommand NavigateToDebugPage { get; set; }
        public RelayCommand NavigateToInfoPage { get; set; }
        public RelayCommand NavigateToInfoGroupDetailPage { get; set; }
        public RelayCommand NavigateToEventsPage { get; set; }
        public RelayCommand NavigateToEventsByDayPage { get; set; }
        public RelayCommand NavigateToEventDetailPage { get; set; }
        public RelayCommand NavigateToLoadingPage { get; set; }
        public RelayCommand NavigateToAboutPage { get; set; }

        public NavigationViewModel(ILayoutPage navigationProvider)
        {
            _navigationProvider = navigationProvider;
            NavigateToMainPage = new RelayCommand(_ => { _navigationProvider.Navigate(typeof(Views.MainPage)); });
            NavigateToDebugPage = new RelayCommand(_ => { _navigationProvider.Navigate(typeof(Views.DebugPage)); });
            NavigateToInfoPage = new RelayCommand(_ => { _navigationProvider.Navigate(typeof(Views.InfoPage)); });
            NavigateToInfoGroupDetailPage = new RelayCommand(p => { _navigationProvider.Navigate(typeof(Views.InfoGroupDetailPage), p); });
            NavigateToEventsPage = new RelayCommand(_ => { _navigationProvider.Navigate(typeof(Views.EventsPage)); });
            NavigateToEventsByDayPage = new RelayCommand(p => { _navigationProvider.Navigate(typeof(Views.EventsByDayPage), p); });
            NavigateToEventDetailPage = new RelayCommand(p => { _navigationProvider.Navigate(typeof(Views.EventDetailPage), p); });
            NavigateToLoadingPage = new RelayCommand(p => { _navigationProvider.Navigate(typeof(Views.LoadingPage), p); });
            NavigateToAboutPage = new RelayCommand(p => { _navigationProvider.Navigate(typeof(Views.AboutPage), p); });
        }

    }
}
