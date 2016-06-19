using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Eurofurence.Companion.Views.Controls
{
    public class FixedFlipView : FlipView
    {
        public int? TargetIndex = -1;

        public ScrollViewer ScrollViewer
        {
            get;
            private set;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.ScrollViewer = (ScrollViewer)this.GetTemplateChild("ScrollingHost");
            this.ScrollViewer.ViewChanged += ScrollViewer_ViewChanged;
        }

        void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var offset = (Math.Round(this.ScrollViewer.HorizontalOffset, 1) + 0.5) - 2;
            var index = (int)offset;

            if (TargetIndex.HasValue)
            {
                if (index != TargetIndex) return;
                TargetIndex = null;
            }

            if (this.SelectedIndex != index)
            {
                this.SelectedIndex = index;
            }
        }
    }
}
