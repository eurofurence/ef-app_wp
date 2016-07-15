using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eurofurence.Companion.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StaticLoadingPage : Page
    {
        public StaticLoadingPage()
        {
            this.InitializeComponent();
            this.Opacity = 0;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        public Task PlayAsync(Storyboard sb)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            EventHandler<object> lambda = null;

            lambda = (s, e) =>
            {
                sb.Completed -= lambda;
                tcs.TrySetResult(null);
            };
            sb.Completed += lambda;
            sb.Begin();

            return tcs.Task;
        }


        public async Task PageFadeInAsync()
        {
            await PlayAsync(pageFadeIn);
        }

        public async Task PageFadeOutAsync()
        {
            await PlayAsync(pageFadeOut);
        }
    }
}
