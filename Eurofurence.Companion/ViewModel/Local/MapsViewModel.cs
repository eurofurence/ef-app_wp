using System.Collections.ObjectModel;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon]
    public class MapsViewModel : BindableBase
    {
        private readonly IMapsViewModelContext _mapsViewModelContext;

        public ObservableCollection<MapViewModel> Maps => _mapsViewModelContext.Maps;

        public MapsViewModel(IMapsViewModelContext mapsViewModelContext)
        {
            _mapsViewModelContext = mapsViewModelContext;
        }
    }
}
