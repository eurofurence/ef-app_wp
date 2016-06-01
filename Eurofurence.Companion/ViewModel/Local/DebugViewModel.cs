using Windows.ApplicationModel;
using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon(Scope = IocBeacon.ScopeEnum.Singleton)]
    public class DebugViewModel : BindableBase
    {
        private bool _isDebugMode;

        public bool IsDebugMode
        {
            get { return _isDebugMode; }
            set { SetProperty(ref _isDebugMode, value); }
        }

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

            return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
