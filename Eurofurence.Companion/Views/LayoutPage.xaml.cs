using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Eurofurence.Companion.Common;
using Windows.UI.Xaml;
using Windows.UI.Core;
using System.Threading.Tasks;

namespace Eurofurence.Companion.Views
{

    public sealed partial class LayoutPage : Page, ILayoutPage
    {
        private ContinuumNavigationTransitionInfo _defaultTransition;

        public Frame RootFrame { get { return _rootFrame; } }

        private Lazy<NavigationViewModel> _navigationViewModel = new Lazy<NavigationViewModel>(() => { return ViewModelLocator.Current.NavigationViewModel; });
        private bool _isMenuVisible = false;
        private bool _isHeaderVisible = true;
        private SearchBarViewModel _searchBarViewModel;

        private readonly INavigationMediator _navigationMediator;
        private readonly ITelemetryClientProvider _telemetryClientProvider;
        private readonly CoreDispatcher _dispatcher;

        private ISearchInteraction CurrentPageSearchInteraction => (RootFrame.Content as ISearchInteraction);

        public bool IsMenuVisible { get { return _isMenuVisible; } set { SetIsMenuVisible(value); } }
        public bool IsHeaderVisible { get { return _isHeaderVisible; } set { SetIsHeaderVisible(value); } }

        private void SetIsHeaderVisible(bool value)
        {
            if (_isHeaderVisible == value) return;

            _isHeaderVisible = value;
            PanelTitle.Visibility = _isHeaderVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetIsMenuVisible(bool value)
        {
            if (_isMenuVisible == value) return;

            _isMenuVisible = value;
            if (_isMenuVisible)
            {
                foreach(var item in _navigationViewModel.Value.MainMenu)
                {
                    item.IsActive = item.ChildTypes?.Contains(RootFrame.Content?.GetType()) ?? false;
                }
                menuShow.Begin();
            } else
            {
                menuHide.Begin();
            }
        }

        public LayoutPage(INavigationMediator navigationMediator, ITelemetryClientProvider telemetryClientProvider)
        {
            InitializeComponent();
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            
            _defaultTransition = new ContinuumNavigationTransitionInfo();
            _menuCompositeRenderTransform.TranslateX = -300;
            _navigationMediator = navigationMediator;
            _telemetryClientProvider = telemetryClientProvider;

            _navigationMediator.OnNavigateAsync += NavigateAsync;
            RootFrame.Navigated += RootFrame_NavigatedAsync;
        }

        private async void RootFrame_NavigatedAsync(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            _searchBarViewModel = new SearchBarViewModel() { Dispatcher = _dispatcher };
            _searchBarViewModel.PropertyChanged += async (s, a) =>
            {
                if (a.PropertyName == nameof(_searchBarViewModel.IsSearchExpanded) && _searchBarViewModel.IsSearchExpanded)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        _textBox_searchBox.Focus(FocusState.Programmatic);
                    });
                }
            };
            _grid_searchBar.DataContext = _searchBarViewModel;


            if (CurrentPageSearchInteraction != null) CurrentPageSearchInteraction.SearchBarViewModel = _searchBarViewModel;
       
            var layoutProperties = (e.Content as ILayoutProperties);
            IsHeaderVisible = layoutProperties?.IsHeaderVisible ?? true;

            //_textBox_searchBox.PlaceholderText = CurrentPageSearchInteraction?.PlaceholderText ?? string.Empty;
            //_textBox_searchBox.Text = CurrentPageSearchInteraction?.DefaultSearchText ?? string.Empty;
                
            //_b_search.Visibility = CurrentPageSearchInteraction != null ? Visibility.Visible : Visibility.Collapsed;
            //IsSearchBoxExpanded = false;


            var pageProperties = (e.Content as IPageProperties);
            if (pageProperties == null) return;

            EventHandler<object> action = null;
            action = (_s, _e) =>
            {
                tbTitle.Text = pageProperties.Title;
                tbIcon.Text = pageProperties.Icon;

                transitionOut.Completed -= action;
                transitionIn.Begin();
            };

            transitionOut.Completed += action;
            transitionOut.Begin();
        }

        public async Task<bool> NavigateAsync(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride, bool forceNewStack = false)
        {
            bool result = false;
            await _dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                if (forceNewStack && !_forceNewStack()) return;
                IsMenuVisible = false;
                _telemetryClientProvider.Client.TrackPageView(sourcePageType.FullName);

                result = RootFrame.Navigate(sourcePageType, parameter, infoOverride ?? _defaultTransition);
            });

            return result;
        }

        [Obsolete]
        public void EnterPage(string area, string title, string subtitle, string icon = "")
        {

        }

        private bool _forceNewStack()
        {
            if (!RootFrame.Navigate(typeof(MainPage))) return false;

            RootFrame.BackStack.Clear();
            RootFrame.ForwardStack.Clear();

            return true;
        }

        private void ContentPresenter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IsMenuVisible = !IsMenuVisible;
        }

        public void OnLayoutPageRendered()
        {
            _menuListView.DataContext = _navigationViewModel.Value.MainMenu;
        }
    }
}
