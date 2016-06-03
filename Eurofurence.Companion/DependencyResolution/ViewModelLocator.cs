using Eurofurence.Companion.ViewModel;
using Eurofurence.Companion.ViewModel.Local;
using Eurofurence.Companion.Views;
using Ninject;

namespace Eurofurence.Companion.DependencyResolution
{
    public class ViewModelLocator
    {
        public static ViewModelLocator Current => KernelResolver.Current.Get<ViewModelLocator>();

        public DebugViewModel DebugViewModel => KernelResolver.Current.Get<DebugViewModel>();
        public EventsViewModel EventsViewModel => KernelResolver.Current.Get<EventsViewModel>();
        public NavigationViewModel NavigationViewModel => KernelResolver.Current.Get<NavigationViewModel>();
        public LayoutViewModel LayoutViewModel => KernelResolver.Current.Get<LayoutViewModel>();
        public InfoViewModel InfoViewModel => KernelResolver.Current.Get<InfoViewModel>();
        public DealersViewModel DealersViewModel => KernelResolver.Current.Get<DealersViewModel>();
        public MainViewModel MainViewModel => KernelResolver.Current.Get<MainViewModel>();
        public MenuViewModel MenuViewModel => KernelResolver.Current.Get<MenuViewModel>();
    }
}