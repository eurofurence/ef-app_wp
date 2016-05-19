using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Eurofurence.Companion.ViewModel
{
    public class MockLayoutPage : ILayoutPage
    {
        public void EnterPage(string area, string title, string subtitle, string icon = "")
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType, bool forceNewBackStack = false)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType, object parameter, bool forceNewBackStack = false)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride, bool forceNewBackStack = false)
        {
            throw new NotImplementedException();
        }

        public void OnLayoutPageRendered()
        {
            throw new NotImplementedException();
        }
    }
}
