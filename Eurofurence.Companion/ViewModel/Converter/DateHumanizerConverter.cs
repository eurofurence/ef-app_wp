using System;
using Windows.UI.Xaml.Data;
using Humanizer;

namespace Eurofurence.Companion.ViewModel.Converter
{
    public class DateHumanizerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime)
            {
                var date = (DateTime) value;
                return date.Humanize();
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
