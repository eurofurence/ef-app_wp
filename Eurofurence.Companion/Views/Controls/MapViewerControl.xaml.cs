using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;
using Ninject;
using NotificationsExtensions.TileContent;

namespace Eurofurence.Companion.Views.Controls
{
    public sealed partial class MapViewerControl : UserControl
    {
        public Visibility MarkerVisibility { get; set; }
        
        public Brush MarkerFill { get; set; }
        public Brush MarkerStroke { get; set; }

        public double MarkerScale { get; set; }

        private bool _isLoaded = false;

        private MapViewModel ViewModel => (DataContext as MapViewModel);

        public event EventHandler MapImageLoadedEvent; 


        public MapViewerControl()
        {
            this.InitializeComponent();
            

            MarkerVisibility = Visibility.Visible;
            MarkerFill = new SolidColorBrush(Color.FromArgb(70, 255, 0, 0));
            MarkerStroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            MarkerScale = 1d;


            if (DesignMode.DesignModeEnabled) return;
            var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;

            DataContextChanged += (sender, args) =>
            {
                if (ViewModel == null || _isLoaded) return;

                _isLoaded = true;
                ServiceLocator.Current.AsyncImageLoaderService.LoadImageAsync(ViewModel.Entity.Image.Id)
                    .ContinueWith(async imageTask =>
                    {
                        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            E_Image_Map.ImageOpened += (s1, a1) => {
                                MapImageLoadedEvent?.Invoke(this, null);
                            };

                            E_Image_Map.Source = imageTask.Result;
                        });
                    });
            };
        }


        private void ImageTapped(object sender, TappedRoutedEventArgs e)
        {
            var position = e.GetPosition((sender as FrameworkElement));

            var closestMatch = ViewModel.Entries.Select(a => new
            {
                Entry = a,
                Distance = Math.Sqrt(
                        Math.Pow(Math.Abs(position.X - a.X), 2) +
                        Math.Pow(Math.Abs(position.Y - a.Y), 2)
                        )
            })
            .OrderBy(a => a.Distance)
            .FirstOrDefault();

            if (closestMatch == null) return;
            
            // More than 45% outside the radius isn't accurate enough.
            if (closestMatch.Distance > closestMatch.Entry.Radius * 1.45) return;

            switch (closestMatch.Entry.Entity.Link.FragmentType)
            {
                case DataModel.Api.LinkFragment.FragmentTypeEnum.DealerDetail:

                    var context = KernelResolver.Current.Get<IDealersViewModelContext>();
                    var dealer = context.Dealers.SingleOrDefault(a => a.Entity.Id.ToString() == closestMatch.Entry.Entity.Link.Target);

                    if (dealer == null) return;
                    ViewModelLocator.Current.NavigationViewModel.NavigateToDealerDetailPage.Execute(dealer);

                    break;
            }
        }

        public void DisposeMapImage()
        {
            E_Image_Map.Source = null;
            GC.Collect();
        }
    }
}
