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

        public event EventHandler<Type> OnPageLoaded;

        public void RaisePageLoaded(Type type)
        {
            OnPageLoaded?.Invoke(this, type);
        }

        public Task<bool> NavigateAsync(Type sourcePageType, bool forceNewStack = false)
            => OnNavigateAsync?.Invoke(sourcePageType, null, null, forceNewStack);

        public Task<bool> NavigateAsync(Type sourcePageType, object parameter, bool forceNewStack = false)
            => OnNavigateAsync?.Invoke(sourcePageType, parameter, null, forceNewStack);

        public Task<bool> NavigateAsync(Type sourcePageType, object parameter, NavigationTransitionInfo transitionInfo, bool forceNewStack = false)
            => OnNavigateAsync?.Invoke(sourcePageType, parameter, transitionInfo, forceNewStack);
    }
}
