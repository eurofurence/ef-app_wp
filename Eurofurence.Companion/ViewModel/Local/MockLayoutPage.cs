using System;
using Windows.UI.Xaml.Media.Animation;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon(TargetType = typeof(ILayoutPage), Scope = IocBeacon.ScopeEnum.Singleton, Environment = IocBeacon.EnvironmentEnum.DesignTimeOnly)]
    public class MockLayoutPage : ILayoutPage
    {
        public bool AcknowledgeNavigateBackRequest()
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType, bool forceNewBackStack = false)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType, object parameter, bool forceNewBackStack = false)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride, bool forceNewBackStack = false)
        {
            throw new NotImplementedException();
        }
    }
}
