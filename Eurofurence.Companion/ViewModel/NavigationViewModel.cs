using Eurofurence.Companion.Common;
using Eurofurence.Companion.DependencyResolution;
using System.Collections.ObjectModel;

namespace Eurofurence.Companion.ViewModel
{
    [IocBeacon(Scope = IocBeacon.ScopeEnum.Singleton)]
    public class NavigationViewModel : BindableBase
    {
        private INavigationMediator _navigationMediator;
        private INavigationProvider _navigationProvider;

        public RelayCommand NavigateToMainPage { get; set; }
        public RelayCommand NavigateToDebugPage { get; set; }
        public RelayCommand NavigateToInfoPage { get; set; }
        public RelayCommand NavigateToInfoGroupDetailPage { get; set; }
        public RelayCommand NavigateToEventsPage { get; set; }
        public RelayCommand NavigateToEventsByDayPage { get; set; }
        public RelayCommand NavigateToEventDetailPage { get; set; }
        public RelayCommand NavigateToLoadingPage { get; set; }
        public RelayCommand NavigateToDealerListPage { get; set; }
        public RelayCommand NavigateToDealerDetailPage { get; set; }
        public RelayCommand NavigateToAboutPage { get; set; }

        public ObservableCollection<NavigationMenuItem> MainMenu => _navigationProvider.MainMenu;

        public NavigationViewModel(INavigationMediator navigationMediator, INavigationProvider navigationProvider)
        {
            _navigationMediator = navigationMediator;
            _navigationProvider = navigationProvider;

            NavigateToMainPage = new RelayCommand(_ => { _navigationMediator.Navigate(typeof(Views.MainPage)); });
            NavigateToDebugPage = new RelayCommand(_ => { _navigationMediator.Navigate(typeof(Views.DebugPage)); });
            NavigateToInfoPage = new RelayCommand(_ => { _navigationMediator.Navigate(typeof(Views.InfoPage)); });
            NavigateToInfoGroupDetailPage = new RelayCommand(p => { _navigationMediator.Navigate(typeof(Views.InfoGroupDetailPage), p); });
            NavigateToEventsPage = new RelayCommand(_ => { _navigationMediator.Navigate(typeof(Views.EventsPage)); });
            NavigateToEventsByDayPage = new RelayCommand(p => { _navigationMediator.Navigate(typeof(Views.EventsByDayPage), p); });
            NavigateToEventDetailPage = new RelayCommand(p => { _navigationMediator.Navigate(typeof(Views.EventDetailPage), p); });
            NavigateToLoadingPage = new RelayCommand(p => { _navigationMediator.Navigate(typeof(Views.LoadingPage), p); });
            NavigateToDealerListPage = new RelayCommand(p => { _navigationMediator.Navigate(typeof(Views.DealerListPage), p); });
            NavigateToDealerDetailPage = new RelayCommand(p => { _navigationMediator.Navigate(typeof(Views.DealerDetailPage), p); });
            NavigateToAboutPage = new RelayCommand(p => { _navigationMediator.Navigate(typeof(Views.AboutPage), p); });
        }
    }
}
