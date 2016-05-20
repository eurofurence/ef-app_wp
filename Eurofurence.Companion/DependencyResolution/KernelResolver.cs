using Ninject;

namespace Eurofurence.Companion.DependencyResolution
{
    public static class KernelResolver
    {
        private static IKernel _kernel;
        public static IKernel Current => _kernel ?? (_kernel = new StandardKernel(new ResolverModule())); 
    }
}
