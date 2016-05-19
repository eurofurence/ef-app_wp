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
