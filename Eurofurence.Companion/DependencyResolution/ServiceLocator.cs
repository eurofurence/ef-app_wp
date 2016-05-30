using Eurofurence.Companion.Common;
using Ninject;
using Windows.UI.Xaml;
using Eurofurence.Companion.Common.Abstractions;

namespace Eurofurence.Companion.DependencyResolution
{
    public class ServiceLocator
    {
        public static ServiceLocator Current => KernelResolver.Current.Get<ServiceLocator>();

        public ITimeProvider TimeProvider => KernelResolver.Current.Get<ITimeProvider>();
    }
}