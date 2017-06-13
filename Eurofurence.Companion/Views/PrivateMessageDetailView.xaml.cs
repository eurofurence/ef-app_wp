using Eurofurence.Companion.Common;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eurofurence.Companion.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PrivateMessageDetailView : Page
    {
        private NavigationHelper navigationHelper;

        private PrivateMessageViewModel _currentMessage;

        public PrivateMessageDetailView()
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



        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            _currentMessage = e.NavigationParameter as PrivateMessageViewModel;
            DataContext = _currentMessage;

            ViewModelLocator.Current.PrivateMessagesViewModel.MarkMessageAsReadCommand.Execute(_currentMessage);
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

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
