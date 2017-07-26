using Eurofurence.Companion.Common;
using System;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.ViewModel.Local;


namespace Eurofurence.Companion.Views
{
    public sealed partial class UserCentralPage : Page , IPageProperties
    {

        public NavigationHelper NavigationHelper { get; }
        private AuthenticationViewModel TypedViewModel { get; }
        private Action _runAfterLogin = null;

        public UserCentralPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += this.NavigationHelper_LoadState;
            NavigationHelper.SaveState += this.NavigationHelper_SaveState;

            TypedViewModel = (DataContext as AuthenticationViewModel);
            TypedViewModel.WatchProperty(nameof(TypedViewModel.IsAuthenticated), (args) =>
            {
                if (_runAfterLogin == null || !TypedViewModel.IsAuthenticated) return;
                _runAfterLogin.Invoke();
                _runAfterLogin = null;
            });
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
            _runAfterLogin = (e.Parameter as Action) ?? _runAfterLogin;
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }


        private async void E_Button_WhyLogin_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var uri = new Uri(@"https://app.eurofurence.org/redir/why-login");
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
