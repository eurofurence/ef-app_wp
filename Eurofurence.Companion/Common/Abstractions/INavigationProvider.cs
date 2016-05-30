using System.Collections.ObjectModel;

namespace Eurofurence.Companion.Common.Abstractions
{
    public interface INavigationProvider
    {
        ObservableCollection<NavigationMenuItem> MainMenu { get; }
    }
}