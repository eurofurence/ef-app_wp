﻿using Ninject;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DependencyResolution;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Eurofurence.Companion.Views
{

    public sealed partial class EventsPage : Page, IPageProperties
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

        public const string PAGE_ICON = "\uE163";
        public string Title => Translations.EventSchedule_Title;
        public string Icon => PAGE_ICON;

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
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