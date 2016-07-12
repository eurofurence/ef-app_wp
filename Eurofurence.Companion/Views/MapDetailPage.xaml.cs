using Eurofurence.Companion.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Eurofurence.Companion.Views
{
    public sealed partial class MapDetailPage : Page
    {
        private readonly NavigationHelper _navigationHelper;

        public MapDetailPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
            _navigationHelper.SaveState += NavigationHelper_SaveState;

            E_ScrollViewer_Map.Loaded += (sender, args) =>
            {
                E_ScrollViewer_Map.ChangeView(null, null, 
                    (float)(E_ScrollViewer_Map.ActualHeight/E_ScrollViewer_Map.ExtentHeight));
            };
        }

        public NavigationHelper NavigationHelper => _navigationHelper;

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                DataContext = e.Parameter;
            }

            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
