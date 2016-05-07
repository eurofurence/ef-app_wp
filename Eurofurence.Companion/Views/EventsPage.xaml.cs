using Ninject;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DependencyResolution;
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

namespace Eurofurence.Companion.Views
{

    public sealed partial class EventsPage : Page
    {
        private const string STATE_EVENTPIVOT_INDEX = "eventPivot.Index";

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        

        public EventsPage()
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
            ViewModelLocator.Current.LayoutViewModel.LayoutPage.EnterPage("", "Event Schedule", "Find out what's going on!", "" + (char)0xE163);

            eventPivot.SelectedIndex = (int)(e.PageState?[STATE_EVENTPIVOT_INDEX] ?? 0);
        }


        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {

            e.PageState.Add(STATE_EVENTPIVOT_INDEX, eventPivot.SelectedIndex);
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

        
        private void ViewByDay_ItemClick(object sender, ItemClickEventArgs e)
        {
            KernelResolver.Current.Get<ViewModelLocator>()
                .NavigationViewModel.NavigateToEventsByDayPage.Execute(e.ClickedItem);
        }
    }
}
