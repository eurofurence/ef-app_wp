using Eurofurence.Companion.Common;
using System;
using System.Windows.Input;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class LinkFragmentAction
    {
        public string TargetTypeName { get; set; }
        public string TargetName { get; set; }

        public Action Execute { get; set; }

        public ICommand Command { get; private set; }

        public LinkFragmentAction()
        {
            Command = new RelayCommand((_) => Execute.Invoke());
        }
    }
}
