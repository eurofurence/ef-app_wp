using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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
    }
}
