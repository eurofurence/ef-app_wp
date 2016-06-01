using Eurofurence.Companion.ViewModel;
using Eurofurence.Companion.ViewModel.Local;

namespace Eurofurence.Companion.Views
{
    public interface IPageProperties
    {
        string Title { get; }
        string Icon { get; }
    }

    public interface ILayoutProperties
    {
        bool IsHeaderVisible { get; }
    }

    public interface ISearchInteraction
    {
        SearchBarViewModel SearchBarViewModel { get; set; }
    }
}
