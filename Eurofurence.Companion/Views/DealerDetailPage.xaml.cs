using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Eurofurence.Companion.Views
{
    public sealed partial class DealerDetailPage : Page, IPageProperties
    {
        private NavigationHelper _navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Dealer _currentDealer;

        public string Icon => "";
        public string Title => _currentDealer?.UiDisplayName;

        public DealerDetailPage()
        {
            this.InitializeComponent();

            this._navigationHelper = new NavigationHelper(this);
            this._navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this._navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper => this._navigationHelper; 

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            _currentDealer = (e.NavigationParameter as Dealer);
            this.DataContext = _currentDealer;        
        }

   
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this._navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this._navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
