using Eurofurence.Companion.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Eurofurence.Companion.Views
{

    public sealed partial class DealerListPage : Page, IPageProperties
    {
        public DealerListPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }


        public NavigationHelper NavigationHelper { get; }


        public ObservableDictionary DefaultViewModel { get; } = new ObservableDictionary();


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
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
