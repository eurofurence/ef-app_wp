using System;
using Windows.UI.Xaml.Media.Animation;

namespace Eurofurence.Companion.Common
{
    public interface INavigationMediator
    {
        event NavigationEvent OnNavigate;

        bool Navigate(Type sourcePageType, bool forceNewStack = false);
        bool Navigate(Type sourcePageType, object parameter, bool forceNewStack = false);
        bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride, bool forceNewStack = false);
    }
}