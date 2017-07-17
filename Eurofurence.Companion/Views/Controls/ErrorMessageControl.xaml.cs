using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Eurofurence.Companion.Views.Controls
{
    public sealed partial class ErrorMessageControl : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("ErrorMessage",
            typeof(string), typeof(ErrorMessageControl), new PropertyMetadata(null, ErrorMessagePropertyChanged));

        private static void ErrorMessagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ErrorMessageControl).UpdateVisibility();
        }

        public string ErrorMessage
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValueDp(TextProperty, value);  }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValueDp(DependencyProperty property, object value, [CallerMemberName]string propertyName = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ErrorMessageControl()
        {
            this.InitializeComponent();
        }

        private void UpdateVisibility()
        {
            if (DesignMode.DesignModeEnabled) return;
            this.Visibility = string.IsNullOrWhiteSpace(ErrorMessage) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
