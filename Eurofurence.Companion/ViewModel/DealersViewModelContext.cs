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
            _dataContext.Refreshed += (s, e) => { MapToViewModels(); };

            Dealers = new ObservableCollection<DealerViewModel>();

            MapToViewModels();
        }

        public event EventHandler Invalidated;

        private void MapToViewModels()
        {
            Dealers.Clear();

            var viewModels = _dataContext.Dealers.Select(entity => new DealerViewModel(entity));

            foreach(var viewModel in viewModels.OrderBy(viewModel => viewModel.DisplayName))
            {
                Dealers.Add(viewModel);
            }

            Invalidated?.Invoke(this, null);
        }
    }
}
