using Windows.UI.Xaml;
using Ninject;
using Eurofurence.Companion.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.ViewModel.Local;
using System.Diagnostics;
using System;

namespace Eurofurence.Companion.Views
{
    public sealed partial class MainPage : Page, IPageProperties
    {
        private ObservableDictionary _defaultViewModel = new ObservableDictionary();

        public string Title => Translations.Main_Title;
        public string Icon => "";

        public MainPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
            NavigationHelper.GoBackCommand = new RelayCommand(async obj => {
                await KernelResolver.Current.Get<IDataContext>().SaveToStoreAsync();
                Application.Current.Exit();
            });

            LogoAnimation.Begin();
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

        private void Banner_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            KernelResolver.Current.Get<NavigationViewModel>().NavigateToDebugPage.Execute(null);
        }
    }
}
