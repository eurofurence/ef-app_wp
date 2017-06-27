using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


namespace Eurofurence.Companion.Views
{
    public sealed partial class InfoGroupDetailPage : Page, IPageProperties
    {
        private KnowledgeEntryViewModel typedViewModel => DataContext as KnowledgeEntryViewModel;

        public InfoGroupDetailPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public string Title => typedViewModel?.Entity.Title ?? "";
        public string Icon => "" + (char)0xEC42;


        public NavigationHelper NavigationHelper { get; }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.NavigationParameter is KnowledgeEntryViewModel)
            {
                var t = e.NavigationParameter as KnowledgeEntryViewModel;
                DataContext = t;
            }
        }

     
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void lvToc_ItemClick(object sender, ItemClickEventArgs e)
        {
            await Task.Delay(1);

            //var cont = icDetail.ContainerFromItem(e.ClickedItem);

            //var transform = (cont as FrameworkElement).TransformToVisual(svMain);
            //var positionInScrollViewer = transform.TransformPoint(new Point(0, 0));

            //svMain.ChangeView(null, positionInScrollViewer.Y, null, false);

        }

        private async void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Task.Delay(1);
            svMain.ChangeView(null, 0, null, false);
        }

        private async void OnWebsiteUriClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var uri = new Uri(((NamedUrl)((FrameworkElement)e.OriginalSource).DataContext).Target);
            await Launcher.LaunchUriAsync(uri);
        }

        private void NextTapped(object sender, TappedRoutedEventArgs e)
        {
            DataContext = typedViewModel.NextEntry;
        }

        private void PreviousTapped(object sender, TappedRoutedEventArgs e)
        {
            DataContext = typedViewModel.PreviousEntry;
        }
    }
}
