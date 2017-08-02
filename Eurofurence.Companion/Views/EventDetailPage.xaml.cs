using Eurofurence.Companion.Common;
using Eurofurence.Companion.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Local;
using Eurofurence.Companion.ViewModel.Local.Entity;

namespace Eurofurence.Companion.Views
{
    public sealed partial class EventDetailPage : Page, IPageProperties
    {
        private EventEntryViewModel _currentEventEntry;

        public string Title => _currentEventEntry?.ConferenceDay?.Entity.WeekdayFullname;
        public string Icon => "\uE184";

        public EventDetailPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;

            E_MapViewerControl_Room.MapImageLoadedEvent += (sender, args) =>
            {
                if (_currentEventEntry?.ConferenceRoom.HasMapEntry ?? false)
                {
                    E_ScrollViewer_Map.ChangeView(_currentEventEntry.ConferenceRoom.MapEntry.X / 1.5
                                                  - (E_ScrollViewer_Map.ActualWidth / 2)
                        , _currentEventEntry.ConferenceRoom.MapEntry.Y / 1.5 - (E_ScrollViewer_Map.ActualHeight / 2), 1 / 1.5f, true);
                }
            };
        }

        public NavigationHelper NavigationHelper { get; }


        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            _currentEventEntry = e.NavigationParameter as EventEntryViewModel;
            DataContext = _currentEventEntry;
        }

       
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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

        private void AppBarToggleButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _currentEventEntry.Entity.AttributesProxy.Extension.IsFavorite = !_currentEventEntry.Entity.AttributesProxy.Extension.IsFavorite;
        }

        private void E_ScrollViewer_Map_OnTapped(object sender, TappedRoutedEventArgs e)
        {
        }
    }
}
