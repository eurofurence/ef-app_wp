using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.ViewModel.Local;
using Eurofurence.Companion.ViewModel.Local.Entity;

namespace Eurofurence.Companion.Views
{
    public sealed partial class EventsByDayPage : Page, IPagePropertiesExtended
    {
        private EventsViewModel _typedViewModel => (EventsViewModel) DataContext;

        private const string STATE_FLIPVIEW_INDEX = "flipview.Index";
        private const string STATE_FLIPVIEW_SCROLLVIEW_VERTICALOFFSET = "flipview.VerticalOffset";


        public EventsByDayPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            NavigationHelper = new NavigationHelper(this);
            //NavigationHelper.LoadState += NavigationHelper_LoadState;
            //NavigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper { get; }

        

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //if (e.PageState == null)
            //{
            //    _isLoaded = true;
            //    return;
            //}

            //RoutedEventHandler flipViewLoaded = null;
            //flipViewLoaded = (_1, __1) =>
            //{
            //    E_FlipView_Events.Loaded -= flipViewLoaded;
            //    E_FlipView_Events.SelectedIndex = e.PageState.ContainsKey(STATE_FLIPVIEW_INDEX) ?
            //        (int)e.PageState[STATE_FLIPVIEW_INDEX] : 0;

            //    _isLoaded = true;

            //    if (e.PageState.ContainsKey(STATE_FLIPVIEW_SCROLLVIEW_VERTICALOFFSET))
            //    {
            //        var selectedItem = (E_FlipView_Events.ContainerFromItem(E_FlipView_Events.SelectedItem) as FlipViewItem);
            //        if (selectedItem == null) return;

            //        RoutedEventHandler restoreVerticalOffset = null;
            //        restoreVerticalOffset = (_2, __2) =>
            //        {
            //            selectedItem.Loaded -= restoreVerticalOffset;
            //            (E_FlipView_Events.ContainerFromItem(E_FlipView_Events.SelectedItem) as FrameworkElement)
            //                .Descendents()
            //                .OfType<ScrollViewer>()
            //                .FirstOrDefault()?
            //                .ChangeView(null, (double)e.PageState[STATE_FLIPVIEW_SCROLLVIEW_VERTICALOFFSET], null, true);
            //        };

            //        selectedItem.Loaded += restoreVerticalOffset;
            //    }
            //};

            //E_FlipView_Events.Loaded += flipViewLoaded;
        }

        private void SelectConferenceDay(EventConferenceDayViewModel conferenceDay, bool forceReset = false)
        {
            if (conferenceDay == null) return;

            foreach (var day in _typedViewModel.EventConferenceDays)
            {
                day.IsSelected = day == conferenceDay;
            }
            TitleChanged?.Invoke(this, conferenceDay.Entity.Name);

            if (E_FlipView_Events.SelectedItem != conferenceDay)
            {
                if (forceReset) E_FlipView_Events.SelectedIndex = -1;
                E_FlipView_Events.SelectedIndex = _typedViewModel.EventConferenceDays.IndexOf(conferenceDay);
                E_FlipView_Events.TargetIndex = E_FlipView_Events.SelectedIndex;
            }
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            //e.PageState.Add(STATE_FLIPVIEW_INDEX, E_FlipView_Events.SelectedIndex);
            //e.PageState.Add(STATE_FLIPVIEW_SCROLLVIEW_VERTICALOFFSET,
            //    (E_FlipView_Events.ContainerFromItem(E_FlipView_Events.SelectedItem) as FrameworkElement)
            //        .Descendents()?
            //        .OfType<ScrollViewer>()?
            //        .FirstOrDefault()?
            //        .VerticalOffset ?? 0);
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is EventConferenceDayViewModel && e.NavigationMode != NavigationMode.Back)
            {
                RoutedEventHandler loaded = null;
                
                loaded = (_1, __1) =>
                {
                    SelectConferenceDay((EventConferenceDayViewModel)e.Parameter, forceReset: true);
                    E_FlipView_Events.Loaded -= loaded;
                };
                E_FlipView_Events.Loaded += loaded;
            }

            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectConferenceDay((EventConferenceDayViewModel) E_FlipView_Events.SelectedItem);
        }

        public event EventHandler<string> TitleChanged;

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var conferenceDay = (EventConferenceDayViewModel)e.ClickedItem;
            if (conferenceDay == null) return;

            SelectConferenceDay(conferenceDay);
        }
    }
}
