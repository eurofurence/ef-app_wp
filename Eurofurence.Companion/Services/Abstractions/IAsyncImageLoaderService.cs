using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Eurofurence.Companion.Services.Abstractions
{
    public interface IAsyncImageLoaderService
    {
        Task<BitmapImage> LoadImageAsync(Guid id);

        void EnqueueAsyncImageLoadTask(Guid id, Image imageControl);
    }
}