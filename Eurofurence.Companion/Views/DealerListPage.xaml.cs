using Eurofurence.Companion.Common;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.ViewModel.Local;

namespace Eurofurence.Companion.Views
{

    public sealed partial class DealerListPage : Page, IPageProperties
    {
        class PageState
        {
            public double ScrollViewerOffsetY { get; set; }
            public bool IsSearchEnabled { get; set; }
            public string SearchText { get; set; }
        }


        public const string PAGE_ICON = "\uE13D";
        public string Title => Translations.Dealers_Title;
        private DealersViewModel TypedViewModel => (DataContext as DealersViewModel);


        public DealerListPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }


        public NavigationHelper NavigationHelper { get; }


        public ObservableDictionary DefaultViewModel { get; } = new ObservableDictionary();


        private ScrollViewer GetListViewScrollViewer()
        {
            E_ListView_Dealers.UpdateLayout();
            return E_ListView_Dealers.Descendents().OfType<ScrollViewer>().FirstOrDefault();
        }


        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (!(e.PageState?.ContainsKey("state") ?? false)) return;
            var state = (e.PageState["state"] as PageState);
            if (state == null) return;

           
            TypedViewModel.IsSearchEnabled = state.IsSearchEnabled;
            TypedViewModel.SearchText = state.SearchText;
            GetListViewScrollViewer().ChangeView(null, state.ScrollViewerOffsetY, null);
        }


        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState["state"] = new PageState()
            {
                IsSearchEnabled = TypedViewModel.IsSearchEnabled,
                SearchText = TypedViewModel.SearchText,
                ScrollViewerOffsetY = GetListViewScrollViewer().VerticalOffset
            };
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

        private void E_AppBar_ToggleSearch_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (E_TextBox_SearchText.Visibility == Visibility.Visible)
                E_TextBox_SearchText.Focus(FocusState.Keyboard);
        }
    }
}
