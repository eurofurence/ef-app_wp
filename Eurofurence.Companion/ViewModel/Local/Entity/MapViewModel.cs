using System.Collections.Generic;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class MapViewModel: EntityBase
    {
        public Map Entity { get; }

        public List<MapEntryViewModel>  Entries { get; }

        public MapViewModel(Map entity)
        {
            InitializeDispatcherFromCurrentThread();

            Entity = entity;
            Entries = new List<MapEntryViewModel>();
        }
    }
}
