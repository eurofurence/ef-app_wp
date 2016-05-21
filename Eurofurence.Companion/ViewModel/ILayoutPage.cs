using System;
using Windows.UI.Xaml.Media.Animation;

namespace Eurofurence.Companion.ViewModel
{
    public interface ILayoutPage
    {
        //bool Navigate(Type sourcePageType, bool forceNewStack = false);
        //bool Navigate(Type sourcePageType, object parameter, bool forceNewStack = false);
        //bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride, bool forceNewStack = false);

        [Obsolete]
        void EnterPage(string area, string title, string subtitle, string icon ="");

        void OnLayoutPageRendered();
    }
}
    