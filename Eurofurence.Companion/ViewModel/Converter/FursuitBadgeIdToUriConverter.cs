using Eurofurence.Companion.Common;
using System;
using Windows.UI.Xaml.Data;

namespace Eurofurence.Companion.ViewModel.Converter
{
    public class FursuitBadgeIdToUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Uri uri = null;

            if (value is Guid)
            {
                return new Uri($"{Consts.WEB_API_ENDPOINT_URL}/Fursuits/Badges/{value}/Image");
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
