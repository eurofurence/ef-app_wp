using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Eurofurence.Companion.Common.Abstractions
{
    public interface INavigationMediator
    {
        event AsyncNavigationEvent OnNavigateAsync;

        event EventHandler<Type> OnPageLoaded;
        void RaisePageLoaded(Type type);

        Task<bool> NavigateAsync(Type sourcePageType, bool forceNewStack = false);
        Task<bool> NavigateAsync(Type sourcePageType, object parameter);
        Task<bool> NavigateAsync(Type sourcePageType, object parameter, bool useTransition = true, bool forceNewStack = false);
    }
}