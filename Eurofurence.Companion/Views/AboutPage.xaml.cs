using Eurofurence.Companion.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Eurofurence.Companion.Views
{
    public sealed partial class AboutPage : Page, IPageProperties
    {
        public string Title => Translations.About_Title;
        public string Icon => "";

        public AboutPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper { get; }


        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }
    }
}
