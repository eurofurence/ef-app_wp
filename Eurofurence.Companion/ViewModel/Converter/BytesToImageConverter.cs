using System;
using System.Text;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Eurofurence.Companion.ViewModel.Converter
{
    public class BytesToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is byte[] || value is string)
            {
                byte[] bytes = value is byte[] ? value as byte[] : Encoding.UTF8.GetBytes((string)value);

                using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
                {

                    using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes(bytes);
                        writer.StoreAsync().GetResults();
                    }

                    var image = new BitmapImage();
                    image.SetSource(ms);
                    return image;
                }
            }

            return null;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
