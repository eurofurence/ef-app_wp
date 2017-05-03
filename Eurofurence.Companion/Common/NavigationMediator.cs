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
            => OnNavigateAsync?.Invoke(sourcePageType, null, forceNewStack: forceNewStack);

        public Task<bool> NavigateAsync(Type sourcePageType, object parameter)
            => OnNavigateAsync?.Invoke(sourcePageType, parameter);

        public Task<bool> NavigateAsync(Type sourcePageType, object parameter, bool useTransition = true, bool forceNewStack = false)
            => OnNavigateAsync?.Invoke(sourcePageType, parameter, useTransition, forceNewStack);
    }
}
