using System;
using Windows.UI.Xaml.Data;

namespace Eurofurence.Companion.ViewModel.Converter
{
    public class UriToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Uri uri = null;

            if (value is string && Uri.IsWellFormedUriString((string)value, UriKind.Absolute))
            {
                uri = new Uri((string)value);
            }

            if (value is Uri)
            {
                uri = (Uri)value;
            }

            if (uri != null)
            {
                return $"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
