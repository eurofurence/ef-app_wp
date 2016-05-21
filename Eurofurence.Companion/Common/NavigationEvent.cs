using System;
using Windows.UI.Xaml.Media.Animation;

namespace Eurofurence.Companion.Common
{
    public delegate bool NavigationEvent(
        Type sourcePageType, 
        object parameter, 
        NavigationTransitionInfo infoOverride,
        bool forceNewStack = false);
}
