using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon]
    public class LayoutViewModel
    {
        public ILayoutPage LayoutPage { get; }

        public LayoutViewModel(ILayoutPage layoutPage)
        {
            LayoutPage = layoutPage;
        }
    }
}
