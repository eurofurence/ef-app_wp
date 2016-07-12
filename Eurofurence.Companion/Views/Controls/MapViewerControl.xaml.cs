using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;
using Ninject;

namespace Eurofurence.Companion.Views.Controls
{
    public sealed partial class MapViewerControl : UserControl
    {
        public Visibility MarkerVisibility { get; set; }
        
        public Brush MarkerFill { get; set; }
        public Brush MarkerStroke { get; set; }

        public double MarkerScale { get; set; }


        public MapViewerControl()
        {
            this.InitializeComponent();

            MarkerVisibility = Visibility.Visible;
            MarkerFill = new SolidColorBrush(Color.FromArgb(70, 255, 0, 0));
            MarkerStroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            MarkerScale = 1d;
        }

        private void MarkerTapped(object sender, TappedRoutedEventArgs e)
        {
            var marker = ((sender as FrameworkElement)?.DataContext as MapEntryViewModel);
            if (marker == null) return;

            switch (marker.Entity.MarkerType)
            {
                case "Dealer":

                    var context = KernelResolver.Current.Get<IDealersViewModelContext>();
                    var dealer = context.Dealers.SingleOrDefault(a => a.Entity.Id == marker.Entity.TargetId);

                    if (dealer == null) return;
                    ViewModelLocator.Current.NavigationViewModel.NavigateToDealerDetailPage.Execute(dealer);

                    break;
            }

        }
    }
}
