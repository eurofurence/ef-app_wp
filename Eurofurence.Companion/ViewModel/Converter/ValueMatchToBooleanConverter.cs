using System;
using Windows.UI.Xaml.Data;

namespace Eurofurence.Companion.ViewModel.Converter
{
    public class ValueMatchToBooleanConverter : IValueConverter
    {
        public bool IsReversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value.ToString().Equals(parameter.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}