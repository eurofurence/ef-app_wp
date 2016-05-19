using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


namespace Eurofurence.Companion.Views
{
    public sealed partial class InfoGroupDetailPage : Page, IPageProperties
    {
        private NavigationHelper navigationHelper;
        private InfoGroup typedViewModel => (this.DataContext as InfoGroup);

        public InfoGroupDetailPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public string Title => typedViewModel?.Name ?? "";
        public string Icon => "" + (char)0xEC42;


        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.NavigationParameter is InfoGroup)
            {
                var t = (e.NavigationParameter as InfoGroup);
                this.DataContext = t;
            }
        }

     
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void lvToc_ItemClick(object sender, ItemClickEventArgs e)
        {
            await Task.Delay(1);

            var cont = icDetail.ContainerFromItem(e.ClickedItem);

            var transform = (cont as FrameworkElement).TransformToVisual(svMain);
            var positionInScrollViewer = transform.TransformPoint(new Point(0, 0));

            svMain.ChangeView(null, positionInScrollViewer.Y, null, false);
            


        }

        private async void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Task.Delay(1);
            svMain.ChangeView(null, 0, null, false);
        }
    }
}
