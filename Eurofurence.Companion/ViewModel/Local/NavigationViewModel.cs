using System.Collections.ObjectModel;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon(Scope = IocBeacon.ScopeEnum.Singleton)]
    public class NavigationViewModel : BindableBase
    {
        private readonly INavigationMediator _navigationMediator;

        public RelayCommand NavigateToMainPage { get; set; }
        public RelayCommand NavigateToDebugPage { get; set; }
        public RelayCommand NavigateToKnowledgePage { get; set; }
        public RelayCommand NavigateToKnowledgeDetailPage { get; set; }
        public RelayCommand NavigateToEventsPage { get; set; }
        public RelayCommand NavigateToEventsByDayPage { get; set; }
        public RelayCommand NavigateToEventsByTrackPage { get; set; }
        public RelayCommand NavigateToEventsByRoomPage { get; set; }
        public RelayCommand NavigateToEventDetailPage { get; set; }
        public RelayCommand NavigateToLoadingPage { get; set; }
        public RelayCommand NavigateToDealerListPage { get; set; }
        public RelayCommand NavigateToDealerDetailPage { get; set; }
        public RelayCommand NavigateToAboutPage { get; set; }
        public RelayCommand NavigateToAnnouncementDetailPage { get; set; }
        public RelayCommand NavigateToImageViewerPage { get; set; }
        public RelayCommand NavigateToUserCentralPage { get; set; }
        public RelayCommand NavigateToPrivateMessageDetailPage { get; set; }

        public RelayCommand NavigateToMapDetailPage { get; set; }

        public NavigationViewModel(INavigationMediator navigationMediator)
        {
            _navigationMediator = navigationMediator;

            NavigateToMainPage = new RelayCommand(_ => { _navigationMediator.NavigateAsync(typeof(Views.MainPage)); });
            NavigateToDebugPage = new RelayCommand(_ => { _navigationMediator.NavigateAsync(typeof(Views.DebugPage)); });
            NavigateToKnowledgePage = new RelayCommand(_ => { _navigationMediator.NavigateAsync(typeof(Views.InfoPage)); });
            NavigateToKnowledgeDetailPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.InfoGroupDetailPage), p); });
            NavigateToEventsPage = new RelayCommand(_ => { _navigationMediator.NavigateAsync(typeof(Views.EventsPage)); });

            NavigateToEventsByDayPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.EventsByDayPage), p); });
            NavigateToEventsByTrackPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.EventsByTrackPage), p); });
            NavigateToEventsByRoomPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.EventsByRoomPage), p); });

            NavigateToEventDetailPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.EventDetailPage), p); });
            NavigateToLoadingPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.LoadingPage), p); });
            NavigateToDealerListPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.DealerListPage), p); });
            NavigateToDealerDetailPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.DealerDetailPage), p); });
            NavigateToAboutPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.AboutPage), p); });
            NavigateToAnnouncementDetailPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.AnnouncementDetailPage), p); });
            NavigateToImageViewerPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.ImageViewerPage), p); });
            NavigateToMapDetailPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.MapDetailPage), p); });

            NavigateToUserCentralPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.UserCentralPage)); });
            NavigateToPrivateMessageDetailPage = new RelayCommand(p => { _navigationMediator.NavigateAsync(typeof(Views.PrivateMessageDetailView), p); });
        }
    }
}
