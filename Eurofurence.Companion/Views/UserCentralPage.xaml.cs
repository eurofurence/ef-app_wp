using Eurofurence.Companion.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Eurofurence.Companion.Views
{
    public sealed partial class UserCentralPage : Page , IPageProperties
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public UserCentralPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += this.NavigationHelper_LoadState;
            NavigationHelper.SaveState += this.NavigationHelper_SaveState;
        }


        public NavigationHelper NavigationHelper { get;  }


        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public string Title => "My Account";


        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

    }
}
