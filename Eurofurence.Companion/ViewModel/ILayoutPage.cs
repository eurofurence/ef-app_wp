using System;

namespace Eurofurence.Companion.ViewModel
{
    public interface ILayoutPage
    {
        [Obsolete]
        void EnterPage(string area, string title, string subtitle, string icon ="");

        void OnLayoutPageRendered();
    }
}
    