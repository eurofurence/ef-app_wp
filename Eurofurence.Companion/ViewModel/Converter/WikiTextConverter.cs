using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;

namespace Eurofurence.Companion.ViewModel.Converter
{
    public class WikiTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is string && targetType == typeof (InlineCollection))
            {
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}