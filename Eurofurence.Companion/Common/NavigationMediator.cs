using Eurofurence.Companion.DependencyResolution;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using Eurofurence.Companion.Common.Abstractions;

namespace Eurofurence.Companion.Common
{
    [IocBeacon(TargetType = typeof(INavigationMediator), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class NavigationMediator : INavigationMediator
    {
        public event AsyncNavigationEvent OnNavigateAsync;

        public Task<bool> NavigateAsync(Type sourcePageType, bool forceNewStack = false)
            => OnNavigateAsync?.Invoke(sourcePageType, null, null, forceNewStack);

        public Task<bool> NavigateAsync(Type sourcePageType, object parameter, bool forceNewStack = false)
            => OnNavigateAsync?.Invoke(sourcePageType, parameter, null, forceNewStack);

        public Task<bool> NavigateAsync(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride, bool forceNewStack = false)
            => OnNavigateAsync?.Invoke(sourcePageType, parameter, infoOverride, forceNewStack);
    }
}
