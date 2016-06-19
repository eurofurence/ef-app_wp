using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel;
using Eurofurence.Companion.ViewModel.Local;

namespace Eurofurence.Companion.Views
{

    public sealed partial class LayoutPage : Page, ILayoutPage
    {
        private readonly INavigationMediator _navigationMediator;
        private readonly ITelemetryClientProvider _telemetryClientProvider;

        private readonly ContinuumNavigationTransitionInfo _defaultTransition;
        private readonly CoreDispatcher _dispatcher;

        private readonly Lazy<MenuViewModel> _menuViewModel = new Lazy<MenuViewModel>(() => ViewModelLocator.Current.MenuViewModel);

        private bool _isHeaderVisible = true;
        private SearchBarViewModel _searchBarViewModel;

        public Frame RootFrame => _rootFrame;

        public bool IsHeaderVisible { get { return _isHeaderVisible; } set { SetIsHeaderVisible(value); } }
        private ISearchInteraction CurrentPageSearchInteraction => RootFrame.Content as ISearchInteraction;

        public LayoutPage(INavigationMediator navigationMediator, ITelemetryClientProvider telemetryClientProvider)
        {
            InitializeComponent();

            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            _navigationMediator = navigationMediator;
            _telemetryClientProvider = telemetryClientProvider;

            _defaultTransition = new ContinuumNavigationTransitionInfo();
            _menuCompositeRenderTransform.TranslateX = -300;


            _navigationMediator.OnNavigateAsync += NavigateAsync;
            RootFrame.Navigated += RootFrame_Navigated;
        }

        public bool AcknowledgeNavigateBackRequest()
        {
            if (!_menuViewModel.Value.IsMenuVisible) return true;
            _menuViewModel.Value.IsMenuVisible = false;
            return false;
        }

        private void SetIsHeaderVisible(bool value)
        {
            if (_isHeaderVisible == value) return;

            _isHeaderVisible = value;
            PanelTitle.Visibility = _isHeaderVisible ? Visibility.Visible : Visibility.Collapsed;
        }


        private void RootFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            //_searchBarViewModel = new SearchBarViewModel() { Dispatcher = _dispatcher };
            //_searchBarViewModel.PropertyChanged += async (s, a) =>
            //{
            //    if (a.PropertyName == nameof(_searchBarViewModel.IsSearchExpanded) && _searchBarViewModel.IsSearchExpanded)
            //    {
            //        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
            //            _textBox_searchBox.Focus(FocusState.Programmatic);
            //        });
            //    }
            //};
            //_grid_searchBar.DataContext = _searchBarViewModel;


            if (CurrentPageSearchInteraction != null) CurrentPageSearchInteraction.SearchBarViewModel = _searchBarViewModel;
       
            var layoutProperties = e.Content as ILayoutProperties;
            IsHeaderVisible = layoutProperties?.IsHeaderVisible ?? true;


            var extendedPageProperties = e.Content as IPagePropertiesExtended;
            if (extendedPageProperties != null)
            {
                extendedPageProperties.TitleChanged += (s1, title) =>
                {
                    SetTitle(title);
                };
            }

            var pageProperties = e.Content as IPageProperties;
            if (pageProperties == null) return;

            SetTitle(pageProperties.Title);;
        }

        private void SetTitle(string title)
        {
            if (title == null || title == string.Empty) return;

            if (tbTitle.Text != title)
            {
                EventHandler<object> action = null;
                action = (_s, _e) =>
                {
                    tbTitle.Text = title;

                    transitionOut.Completed -= action;
                    transitionIn.Begin();
                };

                transitionOut.Completed += action;
                transitionOut.Begin();
            }
        }

        public async Task<bool> NavigateAsync(
            Type sourcePageType, 
            object parameter, 
            NavigationTransitionInfo transitionInfo, 
            bool forceNewStack = false)
        {
            var result = false;

            await _dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                if (forceNewStack && !_forceNewStack()) return;
                _menuViewModel.Value.IsMenuVisible = false;
                _telemetryClientProvider.Client.TrackPageView(sourcePageType.FullName);

                result = RootFrame.Navigate(sourcePageType, parameter, transitionInfo ?? _defaultTransition);
            });

            return result;
        }

        private bool _forceNewStack()
        {
            if (!RootFrame.Navigate(typeof(MainPage))) return false;

            RootFrame.BackStack.Clear();
            RootFrame.ForwardStack.Clear();

            return true;
        }
    }
}
