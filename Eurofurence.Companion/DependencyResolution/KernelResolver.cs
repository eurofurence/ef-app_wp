using Eurofurence.Companion.DataStore;
using Ninject;

namespace Eurofurence.Companion.DependencyResolution
{
    public static class KernelResolver
    {
        private static IKernel _kernel = null;

        public static IKernel Current
        {
            get
            {
                if (_kernel == null)
                {
                    _kernel = new StandardKernel(new ResolverModule());
                }

                return _kernel;
            }
        }
    }
}
