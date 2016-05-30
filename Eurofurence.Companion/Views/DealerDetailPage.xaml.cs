using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Eurofurence.Companion.Views
{
    public sealed partial class DealerDetailPage : Page, IPageProperties
    {
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Dealer _currentDealer;

        public string Icon => "";
        public string Title => _currentDealer?.UiDisplayName;

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
            _currentDealer = e.NavigationParameter as Dealer;
            DataContext = _currentDealer;        
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
    }
}
