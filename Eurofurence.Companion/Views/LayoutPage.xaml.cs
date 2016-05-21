using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Eurofurence.Companion.Common;

namespace Eurofurence.Companion.Views
{

    public sealed partial class LayoutPage : Page, ILayoutPage
    {
        private ContinuumNavigationTransitionInfo _defaultTransition;

        public Frame RootFrame { get { return _rootFrame; } }

        private Lazy<NavigationViewModel> _navigationViewModel = new Lazy<NavigationViewModel>(() => { return ViewModelLocator.Current.NavigationViewModel; });
        private bool _isMenuVisible = false;
        private INavigationMediator _navigationMediator;

        public bool IsMenuVisible { get { return _isMenuVisible; } set { SetIsMenuVisible(value); } }

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

        public LayoutPage(INavigationMediator navigationMediator)
        {
            this.InitializeComponent();
            _defaultTransition = new ContinuumNavigationTransitionInfo();
            _menuCompositeRenderTransform.TranslateX = -300;
            _navigationMediator = navigationMediator;

            _navigationMediator.OnNavigate += Navigate;
            RootFrame.Navigated += RootFrame_Navigated;
        }

        private void RootFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
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

        public bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride, bool forceNewStack = false)
        {
            if (forceNewStack && !_forceNewStack()) return false;
            IsMenuVisible = false;
            return RootFrame.Navigate(sourcePageType, parameter, infoOverride ?? _defaultTransition);
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
