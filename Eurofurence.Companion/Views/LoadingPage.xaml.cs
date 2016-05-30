using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataStore;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Eurofurence.Companion.Views
{
    public sealed partial class LoadingPage : Page, ILayoutProperties, IPageProperties
    {
        public class LoadingPageOptions
        {
            public bool MustCompleteSuccessfully { get; set; }
            public bool AutoStartUpdateOnNavigatedTo { get; set; }
            public bool AutoNavigateBackOnSuccess { get; set; }
        }

        private LoadingPageOptions _options = null;
        private ContextManager _contextManager => DependencyResolution.ViewModelLocator.Current.DebugViewModel.ContextManager;

        public bool IsHeaderVisible => false;
        public string Title => string.Empty;
        public string Icon => string.Empty;

        public LoadingPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
            NavigationHelper.GoBackCommand = new RelayCommand(obj => CanGoBack());
        }

        private void _contextManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_contextManager.UpdateStatus))
            {
                if (_options?.AutoNavigateBackOnSuccess == true &&
                    _contextManager.UpdateStatus == TaskStatus.RanToCompletion)
                {
                    NavigationHelper.GoBack();
                }
            }
        }

        private async void CanGoBack()
        {
            if (_options?.MustCompleteSuccessfully == true && _contextManager.UpdateStatus != TaskStatus.RanToCompletion)
            {
                MessageDialog dlg = new MessageDialog("Sorry, no can do!", "Warning");
                await dlg.ShowAsync();
                return;
            }

            NavigationHelper.GoBack();
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
            _contextManager.PropertyChanged += _contextManager_PropertyChanged;
            _options = e.Parameter as LoadingPageOptions;

            if (_options?.AutoStartUpdateOnNavigatedTo == true)
            {
                _contextManager.UpdateCommand.Execute(null);
            }

            NavigationHelper.OnNavigatedTo(e);
        }

        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _contextManager.PropertyChanged -= _contextManager_PropertyChanged;
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
