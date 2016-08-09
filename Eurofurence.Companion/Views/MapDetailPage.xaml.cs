using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.ViewModel.Local.Entity;


namespace Eurofurence.Companion.Views
{
    public sealed partial class MapDetailPage : Page
    {
        private readonly NavigationHelper _navigationHelper;
        private MapViewModel ViewModel => (DataContext as MapViewModel);

        public MapDetailPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Disabled;

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
            _navigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper => _navigationHelper;

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.Loaded += (s1, e1) =>
            {
                E_Grid_TouchPointNotification.Visibility = (ViewModel?.Entries?.Any() ?? false)
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                var minZoomFactor = (float) (E_ScrollViewer_Map.ActualHeight/ViewModel.Entity.Image.Height);
                E_ScrollViewer_Map.MinZoomFactor = minZoomFactor;


                if (e.PageState != null && e.PageState.ContainsKey("HorizontalOffset"))
                {
                    E_MapViewerControl_Map.MapImageLoadedEvent += (s, args) =>
                    {
                        E_ScrollViewer_Map.ChangeView(
                            (double) e.PageState["HorizontalOffset"],
                            (double) e.PageState["VerticalOffset"],
                            (float) e.PageState["ZoomFactor"],
                            disableAnimation: true
                            );
                    };
                }
                else
                {
                    E_MapViewerControl_Map.MapImageLoadedEvent += (s, args) =>
                    {
                        E_ScrollViewer_Map.ChangeView(
                            null,
                            null,
                            minZoomFactor,
                            disableAnimation: true
                            );
                    };
                }
            };
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState.Add("HorizontalOffset", E_ScrollViewer_Map.HorizontalOffset);
            e.PageState.Add("VerticalOffset", E_ScrollViewer_Map.VerticalOffset);
            e.PageState.Add("ZoomFactor", E_ScrollViewer_Map.ZoomFactor);
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = e.Parameter;
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            E_MapViewerControl_Map.DisposeMapImage();
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
