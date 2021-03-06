﻿using Eurofurence.Companion.Common;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Eurofurence.Companion.Views
{
    public sealed partial class DealerDetailPage : Page, IPageProperties
    {
        private DealerViewModel CurrentDealer => (DataContext as DealerViewModel);

        public string Title => CurrentDealer?.DisplayName;

        public DealerDetailPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;


            E_MapViewerControl_DealersPosition.MapImageLoadedEvent += (sender, args) =>
            {
                if (CurrentDealer?.HasMapEntry ?? false)
                {
                    E_ScrollViewer_Map.ChangeView(CurrentDealer.MapEntry.X / 1.5
                        - (E_ScrollViewer_Map.ActualWidth / 2)
                        , CurrentDealer.MapEntry.Y / 1.5 - (E_ScrollViewer_Map.ActualHeight / 2), 1 / 1.5f, true);
                }
            };
        }

        public NavigationHelper NavigationHelper { get; }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            DataContext = e.NavigationParameter;        
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

        private async void OnWebsiteUriClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var uri = new Uri((string)((FrameworkElement)e.OriginalSource).DataContext);
            await Launcher.LaunchUriAsync(uri);
        }

        private void MapTapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        private async void Twitter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri($"https://twitter.com/{CurrentDealer.Entity.TwitterHandle}"));
        }

        private async void Telegram_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri($"https://telegram.me/{CurrentDealer.Entity.TelegramHandle}"));
        }

        private async void Website_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(CurrentDealer.ParsedWebsiteUris.First());
        }
    }
}
