using Eurofurence.Companion.Common;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Eurofurence.Companion.Views
{

    public sealed partial class DealerListPage : Page, IPageProperties
    {
        private const string STATE_SCROLLVIEWER_OFFSET_Y = "scrollViewer.OffsetY";

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
            var verticalOffset = Convert.ToDouble(e.PageState?[STATE_SCROLLVIEWER_OFFSET_Y] ?? 0);
            GetListViewScrollViewer().ChangeView(null, verticalOffset, null);
        }


        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState[STATE_SCROLLVIEWER_OFFSET_Y] = GetListViewScrollViewer().VerticalOffset;
        }

        public const string PAGE_ICON = "\uE13D";
        public string Title => Translations.Dealers_Title;
        public string Icon => PAGE_ICON;

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
    }
}
