using Ninject;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.Services;

namespace Eurofurence.Companion.DependencyResolution
{
    public class ServiceLocator
    {
        public static ServiceLocator Current => KernelResolver.Current.Get<ServiceLocator>();

        public ITimeProvider TimeProvider => KernelResolver.Current.Get<ITimeProvider>();

        public EventService EventService => KernelResolver.Current.Get<EventService>();
    }
}