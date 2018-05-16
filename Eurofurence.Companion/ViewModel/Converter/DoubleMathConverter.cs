using System;
using Windows.UI.Xaml.Data;

namespace Eurofurence.Companion.ViewModel.Converter
{
    public class DoubleMathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var doubleVal = System.Convert.ToDouble(value);
            var doubleParam = System.Convert.ToDouble(parameter);

            return Math.Abs(doubleVal + doubleParam);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}