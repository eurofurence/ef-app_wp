using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.ViewModel
{
    [IocBeacon]
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
