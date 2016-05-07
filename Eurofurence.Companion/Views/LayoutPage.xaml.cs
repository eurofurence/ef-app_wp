using Eurofurence.Companion.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class LayoutPage : Page, ILayoutPage
    {
        private ContinuumNavigationTransitionInfo _defaultTransition;

        public Frame RootFrame { get { return _rootFrame; } }

        public LayoutPage()
        {
            this.InitializeComponent();
            _defaultTransition = new ContinuumNavigationTransitionInfo();

        }

  

        public bool Navigate(Type sourcePageType)
        {
            return RootFrame.Navigate(sourcePageType, null, _defaultTransition);
        }

        public bool Navigate(Type sourcePageType, object parameter)
        {
            return RootFrame.Navigate(sourcePageType, parameter, _defaultTransition);
        }

        public bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride)
        {
            return RootFrame.Navigate(sourcePageType, parameter, infoOverride);
        }

        public void EnterPage(string area, string title, string subtitle, string icon = "")
        {
            //var noHeader = String.IsNullOrWhiteSpace(area) && String.IsNullOrWhiteSpace(title) && String.IsNullOrWhiteSpace(subtitle);
            //if (noHeader && PanelTitle.ActualHeight > 0)
            //{
            //    headerOut.Begin();
            //}

            //if (!noHeader && PanelTitle.ActualHeight < 55)
            //{
            //    headerIn.Begin();
            //}

            EventHandler<object> action = null;
            action = (object sender, object e) =>
            {
                //tbArea.Text = area;
                tbTitle.Text = title.ToUpperInvariant();
                tbIcon.Text = icon;
                //tbSubtitle.Text = subtitle;

                transitionOut.Completed -= action;
                transitionIn.Begin();
            };
                       

            transitionOut.Completed += action;
            transitionOut.Begin();
        }


        private void TransitionOut_Completed(object sender, object e)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            headerIn.Begin();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            headerOut.Begin();
        }


    }
}
