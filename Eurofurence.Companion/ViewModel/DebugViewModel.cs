using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using Windows.ApplicationModel;

namespace Eurofurence.Companion.ViewModel
{
    [IocBeacon]
    public class DebugViewModel : BindableBase
    {
        public ContextManager ContextManager { get; private set; }

        public string AppVersion => GetAppVersion();

        public DebugViewModel(ContextManager contextManager)
        {
            ContextManager = contextManager;
        }

        private string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }
    }
}
