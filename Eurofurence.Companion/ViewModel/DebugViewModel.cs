using Eurofurence.Companion.DataStore;

namespace Eurofurence.Companion.ViewModel
{
    public class DebugViewModel : BindableBase
    {
        public ContextManager ContextManager { get; private set; }

        public DebugViewModel(ContextManager contextManager)
        {
            ContextManager = contextManager;
        }
    }
}
