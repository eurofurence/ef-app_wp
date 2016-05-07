using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace Eurofurence.Companion.ViewModel
{
    public class MockNavigationProvider : ILayoutPage
    {
        public void EnterPage(string area, string title, string subtitle, string icon = "")
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType, object parameter)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride)
        {
            throw new NotImplementedException();
        }
    }
}
