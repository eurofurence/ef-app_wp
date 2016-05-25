using Eurofurence.Companion.Common;
using Ninject;

namespace Eurofurence.Companion.DependencyResolution
{
    public class ServiceLocator
    {
        public static ServiceLocator Current => KernelResolver.Current.Get<ServiceLocator>();

        public ITimeProvider TimeProvider => KernelResolver.Current.Get<ITimeProvider>();
    }
}