using System;
using Windows.UI.Xaml.Media.Animation;

namespace Eurofurence.Companion.ViewModel
{
    public interface ILayoutPage
    {
        bool Navigate(Type sourcePageType);
        bool Navigate(Type sourcePageType, object parameter);
        bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride);

        void EnterPage(string area, string title, string subtitle, string icon ="");
    }
}
    