using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Ninject;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Eurofurence.Companion.Views
{
    public sealed partial class FirstStartPage : Page, ILayoutProperties
    {
        private NavigationHelper _navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public bool IsHeaderVisible => false;

        public FirstStartPage()
        {
            InitializeComponent();

            _navigationHelper = new NavigationHelper(this);
            _navigationHelper.GoBackCommand = new RelayCommand(obj => App.Current.Exit());
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

        private async void ButtonStartDownload_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _navigationHelper.GoBackCommand = null;
            _navigationHelper.GoBack();

            var navigationMediator = KernelResolver.Current.Get<INavigationMediator>();

            await navigationMediator.NavigateAsync(
                      typeof(LoadingPage),
                      new LoadingPage.LoadingPageOptions
                      {
                          MustCompleteSuccessfully = true,
                          AutoNavigateBackOnSuccess = true,
                          AutoStartUpdateOnNavigatedTo = true
                      });
        }

        private void ButtonCloseApplication_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.Current.Exit();
        }
    }
}
