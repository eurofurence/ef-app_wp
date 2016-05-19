using System.Collections.ObjectModel;

namespace Eurofurence.Companion.Common
{
    public interface INavigationProvider
    {
        ObservableCollection<NavigationMenuItem> MainMenu { get; }
    }
}