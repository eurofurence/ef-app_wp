using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.Common.Abstractions;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon(Scope = IocBeacon.ScopeEnum.Singleton)]
    public class DebugViewModel : BindableBase
    {
        private bool _isDebugMode;
        private IAppVersionProvider _appVersionProvider;

        public bool IsDebugMode
        {
            get { return _isDebugMode; }
            set { SetProperty(ref _isDebugMode, value); }
        }

        public ContextManager ContextManager { get; private set; }

        public string AppVersion => _appVersionProvider.GetAppVersion();

        public DebugViewModel(ContextManager contextManager, IAppVersionProvider appVersionProvider)
        {
            InitializeDispatcherFromCurrentThread();
            ContextManager = contextManager;
            _appVersionProvider = appVersionProvider;
        }
    }
}
