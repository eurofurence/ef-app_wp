using Eurofurence.Companion.ViewModel;
using Eurofurence.Companion.ViewModel.Local;
using Eurofurence.Companion.Views;
using Ninject;
using Windows.ApplicationModel;
using Windows.UI.Core;

namespace Eurofurence.Companion.DependencyResolution
{
    public class ViewModelLocator
    {
        public static ViewModelLocator Current => KernelResolver.Current.Get<ViewModelLocator>();

        public DebugViewModel DebugViewModel => KernelResolver.Current.Get<DebugViewModel>();
        public EventsViewModel EventsViewModel => KernelResolver.Current.Get<EventsViewModel>();
        public NavigationViewModel NavigationViewModel => KernelResolver.Current.Get<NavigationViewModel>();
        public LayoutViewModel LayoutViewModel => KernelResolver.Current.Get<LayoutViewModel>();
        public ConventionInfoViewModel ConventionInfoViewModel => KernelResolver.Current.Get<ConventionInfoViewModel>();
        public DealersViewModel DealersViewModel => KernelResolver.Current.Get<DealersViewModel>();
        public UpcomingEventsViewModel UpcomingEventsViewModel => KernelResolver.Current.Get<UpcomingEventsViewModel>();
        public ActiveAnnouncementsViewModel ActiveAnnouncementsViewModel => KernelResolver.Current.Get<ActiveAnnouncementsViewModel>();
        public ConventionStateViewModel ConventionStateViewModel => KernelResolver.Current.Get<ConventionStateViewModel>();
        public MenuViewModel MenuViewModel => KernelResolver.Current.Get<MenuViewModel>();
        public MapsViewModel MapsViewModel => KernelResolver.Current.Get<MapsViewModel>();
        public AuthenticationViewModel AuthenticationViewModel => KernelResolver.Current.Get<AuthenticationViewModel>();
        public PrivateMessagesViewModel PrivateMessagesViewModel => KernelResolver.Current.Get<PrivateMessagesViewModel>();
        public NetworkConnectivityViewModel NetworkConnectivityViewModel => KernelResolver.Current.Get<NetworkConnectivityViewModel>();

        public CollectionGameManageFursuitsViewModel CollectionGameManageFursuitsViewModel => KernelResolver.Current.Get<CollectionGameManageFursuitsViewModel>();
        public CollectionGamePlayerViewModel CollectionGamePlayerViewModel => KernelResolver.Current.Get<CollectionGamePlayerViewModel>();
        public CollectionGameScoreboardViewModel CollectionGameScoreboardViewModel => KernelResolver.Current.Get<CollectionGameScoreboardViewModel>();
    }
}