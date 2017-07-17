using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Eurofurence.Companion.ViewModel.Converter
{

    public class ValueMatchToVisibilityConverter : IValueConverter
    {
        public bool IsReversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Let's make things always visible in the designer.
            if (DesignMode.DesignModeEnabled) return Visibility.Visible;

            return (value.ToString().Equals(parameter.ToString())) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
