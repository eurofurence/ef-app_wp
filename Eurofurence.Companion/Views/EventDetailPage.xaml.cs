﻿using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Eurofurence.Companion.Views
{
    public sealed partial class EventDetailPage : Page, IPageProperties
    {
        private NavigationHelper _navigationHelper;
        private EventEntry _currentEventEntry;

        public string Title => _currentEventEntry?.ConferenceDay?.WeekdayFullname;
        public string Icon => "\uE184";

        public EventDetailPage()
        {
            this.InitializeComponent();

            this._navigationHelper = new NavigationHelper(this);
            this._navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this._navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper => _navigationHelper;

        
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            _currentEventEntry = (e.NavigationParameter as EventEntry);
            this.DataContext = _currentEventEntry;
        }

       
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this._navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this._navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}