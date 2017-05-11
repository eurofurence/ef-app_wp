using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Eurofurence.Companion.ViewModel
{
    [IocBeacon(TargetType = typeof(IDealersViewModelContext), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class DealersViewModelContext : BindableBase, IDealersViewModelContext
    {
        private IDataContext _dataContext;

        public ObservableCollection<DealerViewModel> Dealers { get; }

        public DealersViewModelContext(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _dataContext.Refreshed += (s, e) => { if (e.HasFlag(DataContextDataAreaEnum.Dealers)) MapToViewModels(); };

            Dealers = new ObservableCollection<DealerViewModel>();

            MapToViewModels();
        }

        public event EventHandler Invalidated;

        private void MapToViewModels()
        {
            Dealers.Clear();

            var viewModels = _dataContext.Dealers
                .Select(entity => new DealerViewModel(entity));

            foreach(var viewModel in viewModels.OrderBy(viewModel => viewModel.DisplayName))
            {
                var mapEntry = _dataContext.MapEntries.SingleOrDefault(
                    a => a.MarkerType == "Dealer" && a.TargetId == viewModel.Entity.Id);

                if (mapEntry != null)
                {
                    viewModel.MapEntry = new MapEntryViewModel(mapEntry);
                    viewModel.MapEntry.Map = new MapViewModel(mapEntry.Map);
                    viewModel.MapEntry.Map.Entries.Add(viewModel.MapEntry);
                }

                Dealers.Add(viewModel);
            }

            Invalidated?.Invoke(this, null);
        }
    }
}
