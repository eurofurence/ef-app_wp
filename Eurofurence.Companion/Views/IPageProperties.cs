using System;
using Eurofurence.Companion.ViewModel.Local;

namespace Eurofurence.Companion.Views
{
    public interface IPageProperties
    {
        string Title { get; }
    }

    public interface IPagePropertiesExtended
    {
        event EventHandler<string> TitleChanged;
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
