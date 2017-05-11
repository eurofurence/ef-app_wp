using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Eurofurence.Companion.ViewModel
{

    [IocBeacon(TargetType = typeof(IMapsViewModelContext), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class MapsViewModelContext : BindableBase, IMapsViewModelContext
    {
        private readonly IDataContext _dataContext;

        public ObservableCollection<MapViewModel> Maps { get; }

        public MapsViewModelContext(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _dataContext.Refreshed += (s, e) => { if (e.HasFlag(DataContextDataAreaEnum.Maps)) MapToViewModels(); };

            Maps = new ObservableCollection<MapViewModel>();

            MapToViewModels();
        }

        public event EventHandler Invalidated;

        private void MapToViewModels()
        {
            Maps.Clear();

            var viewModels = _dataContext.Maps.Select(entity => new MapViewModel(entity));

            foreach(var viewModel in viewModels.OrderBy(viewModel => viewModel.Entity.Description))
            {
                viewModel.Entries.AddRange(viewModel.Entity.Entries.Select(
                    entity => new MapEntryViewModel(entity) { Map = viewModel }));
                Maps.Add(viewModel);
            }

            Invalidated?.Invoke(this, null);
        }
    }
}
