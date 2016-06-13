using System.Collections.ObjectModel;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon]
    public class ConventionInfoViewModel : BindableBase
    {
        private IInfoViewModelContext _infoViewModelContext;

        public ObservableCollection<InfoGroupViewModel> Groups => _infoViewModelContext.Groups;


        public ConventionInfoViewModel(IInfoViewModelContext infoViewModelContext)
        {
            _infoViewModelContext = infoViewModelContext;
            _infoViewModelContext.Invalidated += (s, e) => { OnPropertyChanged(nameof(Groups)); };
        }
    }
}
