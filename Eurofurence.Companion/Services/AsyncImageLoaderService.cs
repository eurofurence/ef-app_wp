using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.Services.Abstractions;
using Eurofurence.Companion.ViewModel.Converter;

namespace Eurofurence.Companion.Services
{
    [IocBeacon(Scope = IocBeacon.ScopeEnum.Singleton, Environment = IocBeacon.EnvironmentEnum.Any, TargetType = typeof(IAsyncImageLoaderService))]
    public class AsyncImageLoaderService : IAsyncImageLoaderService
    {
        private readonly IDataStore _dataStore;
        private readonly ConcurrentQueue<Tuple<Guid, Image>> _asyncQueue = new ConcurrentQueue<Tuple<Guid, Image>>();
        private readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public AsyncImageLoaderService(IDataStore dataStore)
        {
            _dataStore = dataStore;
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
            _dispatcherTimer.Start();
            _dispatcherTimer.Tick += DispatcherTimerOnTick;
        }

        public async Task<BitmapImage> LoadImageAsync(Guid id)
        {
            await _semaphore.WaitAsync();
            var imageStream = await _dataStore.GetBlobStreamAsync(id, "ImageData");
            _semaphore.Release();

            Debug.WriteLine($"Streaming {imageStream.Size} bytes for {id}");

            var image = (new StreamToImageConverter().Convert(imageStream, null, null, "") as BitmapImage);

            return image;
        }

        public void EnqueueAsyncImageLoadTask(Guid id, Image imageControl)
        {
            _asyncQueue.Enqueue(new Tuple<Guid, Image>(id, imageControl));
        }

        private async void DispatcherTimerOnTick(object sender, object o)
        {
            if (!_asyncQueue.IsEmpty)
            {
                Tuple<Guid, Image> t = null;
                if (!_asyncQueue.TryDequeue(out t)) return;
                
                t.Item2.Source = await LoadImageAsync(t.Item1);
            }
        }
    }
}
