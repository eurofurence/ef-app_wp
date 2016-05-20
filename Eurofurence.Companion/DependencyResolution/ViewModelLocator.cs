using Eurofurence.Companion.ViewModel;
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
    }
}