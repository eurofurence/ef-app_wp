using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.ViewModel.Local;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Eurofurence.Companion.Views
{
    public sealed partial class DealerDetailPage : Page, IPageProperties
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private DealerViewModel CurrentDealer => (DataContext as DealerViewModel);

        public string Icon => "";
        public string Title => CurrentDealer?.DisplayName;

        public DealerDetailPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper { get; }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            DataContext = e.NavigationParameter;        
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

        private async void OnWebsiteUriClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var uri = new Uri((string)((FrameworkElement)e.OriginalSource).DataContext);
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
