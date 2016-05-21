using System;
using Windows.UI.Xaml.Media.Animation;

namespace Eurofurence.Companion.Common
{
    public class NavigationMediator : INavigationMediator
    {
        public event NavigationEvent OnNavigate;

        public bool Navigate(Type sourcePageType, bool forceNewStack = false) 
            => OnNavigate?.Invoke(sourcePageType, null, null, forceNewStack) ?? false;

        public bool Navigate(Type sourcePageType, object parameter, bool forceNewStack = false) 
            => OnNavigate?.Invoke(sourcePageType, parameter, null, forceNewStack) ?? false;

        public bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride, bool forceNewStack = false)
            => OnNavigate?.Invoke(sourcePageType, parameter, infoOverride, forceNewStack) ?? false;
    }
}
