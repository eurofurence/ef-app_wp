using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DependencyResolution;


namespace Eurofurence.Companion.Views
{
    public sealed partial class ImageViewerPage : Page
    {
        private readonly NavigationHelper _navigationHelper;
        public bool IsHeaderVisible => false;

        public ImageViewerPage()
        {
            InitializeComponent();

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.LoadState += NavigationHelper_LoadState;
            _navigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper => _navigationHelper;

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.NavigationParameter is ImageSource)
            {
                E_Image.Source = (e.NavigationParameter as ImageSource);
            } else if (e.NavigationParameter is Guid)
            {
                E_Image.Source =
                    await
                        ServiceLocator.Current.AsyncImageLoaderService
                            .LoadImageAsync((Guid) e.NavigationParameter);
            }
        }


        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void E_Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SV_ImageZoom.ChangeView(null, null, 1.0f, false);
        }
    }
}
