using Eurofurence.Companion.Common;
using Eurofurence.Companion.DependencyResolution;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Eurofurence.Companion.Views
{

    public sealed partial class DealerListPage : Page, IPageProperties
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public DealerListPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }


        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

 
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }


        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        public const string PAGE_ICON = "\uE13D";
        public string Title => Translations.Dealers_Title;
        public string Icon => PAGE_ICON;

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
    }
}
