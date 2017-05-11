using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI;

namespace Eurofurence.Companion.DataModel
{
    public static class ImageExtensions
    {
        public static async Task<Color> GetDominantColorAsync(this DataModel.Api.Image entity)
        {
            using (var ms = new InMemoryRandomAccessStream())
            {
                using (var writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(entity.Content);
                    writer.StoreAsync().GetResults();
                }

                var decoder = await BitmapDecoder.CreateAsync(ms);
                var myTransform = new BitmapTransform { ScaledHeight = 1, ScaledWidth = 1 };
                var pixels = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Rgba8,
                    BitmapAlphaMode.Ignore,
                    myTransform,
                    ExifOrientationMode.IgnoreExifOrientation,
                    ColorManagementMode.DoNotColorManage);
                var bytes = pixels.DetachPixelData();

                return Color.FromArgb(255, bytes[0], bytes[1], bytes[2]);
            }
        }
    }
}
