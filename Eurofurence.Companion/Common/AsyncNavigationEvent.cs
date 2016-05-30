using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Eurofurence.Companion.Common
{
    public delegate Task<bool> AsyncNavigationEvent(
        Type sourcePageType, 
        object parameter, 
        NavigationTransitionInfo infoOverride,
        bool forceNewStack = false);
}
