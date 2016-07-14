using Ninject;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.Services;
using Eurofurence.Companion.Services.Abstractions;

namespace Eurofurence.Companion.DependencyResolution
{
    public class ServiceLocator
    {
        public static ServiceLocator Current => KernelResolver.Current.Get<ServiceLocator>();

        public ITimeProvider TimeProvider => KernelResolver.Current.Get<ITimeProvider>();

        public EventService EventService => KernelResolver.Current.Get<EventService>();

        public IAsyncImageLoaderService AsyncImageLoaderService
            => KernelResolver.Current.Get<IAsyncImageLoaderService>();
    }
}