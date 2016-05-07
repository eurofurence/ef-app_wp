using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eurofurence.Companion.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingPage : Page
    {
        public class LoadingPageOptions
        {
            public bool MustCompleteSuccessfully { get; set; }
            public bool AutoStartUpdateOnNavigatedTo { get; set; }
            public bool AutoNavigateBackOnSuccess { get; set; }
        }

        private LoadingPageOptions _options = null;
        private ContextManager _contextManager => DependencyResolution.ViewModelLocator.Current.DebugViewModel.ContextManager;

        private NavigationHelper navigationHelper;

        public LoadingPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            this.navigationHelper.GoBackCommand = new RelayCommand((obj) => this.CanGoBack());
        }

        private void _contextManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_contextManager.UpdateStatus))
            {
                if (_options?.AutoNavigateBackOnSuccess == true &&
                    _contextManager.UpdateStatus == TaskStatus.RanToCompletion)
                {
                    this.navigationHelper.GoBack();
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

            this.navigationHelper.GoBack();
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

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
            _options = (e.Parameter as LoadingPageOptions);

            if (_options?.AutoStartUpdateOnNavigatedTo == true)
            {
                _contextManager.UpdateCommand.Execute(null);
            }

            this.navigationHelper.OnNavigatedTo(e);
        }

        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _contextManager.PropertyChanged -= _contextManager_PropertyChanged;
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
