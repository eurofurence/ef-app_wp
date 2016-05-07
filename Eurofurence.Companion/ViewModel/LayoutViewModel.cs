using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eurofurence.Companion.ViewModel
{
    public class LayoutViewModel
    {
        private ILayoutPage _layoutPage;
        public ILayoutPage LayoutPage => _layoutPage;

        public LayoutViewModel(ILayoutPage layoutPage)
        {
            _layoutPage = layoutPage;
        }
    }
}
