using Ninject;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DependencyResolution;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.ViewModel.Local;
using Windows.UI.Xaml;
using System.Linq;

namespace Eurofurence.Companion.Views
{

    public sealed partial class EventsPage : Page, IPageProperties

    {
        private const string STATE_FLIPVIEW_INDEX = "flipview.Index";
        private const string STATE_FLIPVIEW_SCROLLVIEW_VERTICALOFFSET = "flipview.VerticalOffset";
        private const string STATE_SEARCHTEXT = "searchText";

        private EventsViewModel ViewModel => (EventsViewModel)DataContext;

        public EventsPage()
        {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Enabled;

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }


        public NavigationHelper NavigationHelper { get; }

        public const string PAGE_ICON = "\uE163";

        public string Title => Translations.EventSchedule_Title;
        public string Icon => PAGE_ICON;

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState == null) return;

            ViewModel.SearchText = (string)e.PageState[STATE_SEARCHTEXT];

            GroupFlipView.SelectedIndex = e.PageState.ContainsKey(STATE_FLIPVIEW_INDEX) ?
                (int)e.PageState[STATE_FLIPVIEW_INDEX] : 0;

            if (e.PageState.ContainsKey(STATE_FLIPVIEW_SCROLLVIEW_VERTICALOFFSET))
            {
                var selectedItem = (GroupFlipView.SelectedItem as FlipViewItem);
                RoutedEventHandler restoreVerticalOffset = null;
                restoreVerticalOffset = (_, __) =>
                {
                    selectedItem.Loaded -= restoreVerticalOffset;
                    (GroupFlipView.SelectedItem as FrameworkElement)
                        .Descendents()
                        .OfType<ScrollViewer>()
                        .FirstOrDefault()?
                        .ChangeView(null, (double)e.PageState[STATE_FLIPVIEW_SCROLLVIEW_VERTICALOFFSET], null, true);
                };

                selectedItem.Loaded += restoreVerticalOffset;
            }
        }



        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState.Add(STATE_SEARCHTEXT, ViewModel.SearchText);
            e.PageState.Add(STATE_FLIPVIEW_INDEX, GroupFlipView.SelectedIndex);
            e.PageState.Add(STATE_FLIPVIEW_SCROLLVIEW_VERTICALOFFSET, 
                (GroupFlipView.SelectedItem as FrameworkElement)
                    .Descendents()?
                    .OfType<ScrollViewer>()?
                    .FirstOrDefault()?
                    .VerticalOffset ?? 0);
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

        
        private void ViewByDay_ItemClick(object sender, ItemClickEventArgs e)
        {
            KernelResolver.Current.Get<ViewModelLocator>()
                .NavigationViewModel.NavigateToEventsByDayPage.Execute(e.ClickedItem);
        }

        private void GroupItemClick(object sender, ItemClickEventArgs e)
        {
            var t = (e.ClickedItem as EventsPageViewModel.EventGroupViewModel);

            GroupFlipView.SelectedIndex = localViewModel.Groups.IndexOf(t);
            foreach(var v in localViewModel.Groups)
            {
                v.IsSelected = (v == t);
            }
        }

        private void SynchronizeFlipView(object sender, SelectionChangedEventArgs e)
        {
            if (localViewModel == null) return;
            foreach (var v in localViewModel.Groups)
            {
                v.IsSelected = localViewModel.Groups.IndexOf(v) == GroupFlipView.SelectedIndex;
            }
        }
    }
}
