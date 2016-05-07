using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.ViewModel;
using Ninject.Modules;
using Windows.ApplicationModel;

namespace Eurofurence.Companion.DependencyResolution
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ContextManager>().To<ContextManager>().InSingletonScope();
            Bind<INavigationResolver>().To<NavigationResolver>().InSingletonScope();
            Bind<IApplicationSettingsManager>().To<ApplicationSettingsManager>();
            Bind<ApplicationSettingsContext>().To<ApplicationSettingsContext>();


            if (DesignMode.DesignModeEnabled)
            {
                Bind<IDataContext>().To<MockDataContext>().InSingletonScope();
                Bind<ILayoutPage>().To<MockNavigationProvider>().InSingletonScope();
                Bind<IDataStore>().To<RealtimeApiAccessDataStore>().InSingletonScope();
            }
            else
            {
                Bind<IDataContext>().To<ObservableDataContext>().InSingletonScope();
                Bind<IDataStore>().To<SqliteDataStore>().InSingletonScope();
            }

            Bind<DebugViewModel>().To<DebugViewModel>();
            Bind<EventsViewModel>().To<EventsViewModel>().InSingletonScope();
            Bind<NavigationViewModel>().To<NavigationViewModel>();
            Bind<LayoutViewModel>().To<LayoutViewModel>();
            Bind<InfoViewModel>().To<InfoViewModel>();
            Bind<DealersViewModel>().To<DealersViewModel>();
        }
    }
}

