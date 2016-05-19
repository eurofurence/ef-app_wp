using Eurofurence.Companion.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Eurofurence.Companion.ViewModel
{


    public class NavigationViewModel : BindableBase
    {
        private ILayoutPage _layoutPage;
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


        public NavigationViewModel(ILayoutPage layoutPage, INavigationProvider navigationProvider)
        {
            _layoutPage = layoutPage;
            _navigationProvider = navigationProvider;

            NavigateToMainPage = new RelayCommand(_ => { _layoutPage.Navigate(typeof(Views.MainPage)); });
            NavigateToDebugPage = new RelayCommand(_ => { _layoutPage.Navigate(typeof(Views.DebugPage)); });
            NavigateToInfoPage = new RelayCommand(_ => { _layoutPage.Navigate(typeof(Views.InfoPage)); });
            NavigateToInfoGroupDetailPage = new RelayCommand(p => { _layoutPage.Navigate(typeof(Views.InfoGroupDetailPage), p); });
            NavigateToEventsPage = new RelayCommand(_ => { _layoutPage.Navigate(typeof(Views.EventsPage)); });
            NavigateToEventsByDayPage = new RelayCommand(p => { _layoutPage.Navigate(typeof(Views.EventsByDayPage), p); });
            NavigateToEventDetailPage = new RelayCommand(p => { _layoutPage.Navigate(typeof(Views.EventDetailPage), p); });
            NavigateToLoadingPage = new RelayCommand(p => { _layoutPage.Navigate(typeof(Views.LoadingPage), p); });
            NavigateToDealerListPage = new RelayCommand(p => { _layoutPage.Navigate(typeof(Views.DealerListPage), p); });
            NavigateToDealerDetailPage = new RelayCommand(p => { _layoutPage.Navigate(typeof(Views.DealerDetailPage), p); });
            NavigateToAboutPage = new RelayCommand(p => { _layoutPage.Navigate(typeof(Views.AboutPage), p); });
            
            
        }

    }
}
