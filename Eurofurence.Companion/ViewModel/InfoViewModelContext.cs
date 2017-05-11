using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Eurofurence.Companion.ViewModel
{
    [IocBeacon(TargetType = typeof(IInfoViewModelContext), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class InfoViewModelContext : BindableBase, IInfoViewModelContext
    {
        private IDataContext _dataContext;

        public ObservableCollection<InfoGroupViewModel> Groups { get; }

        public InfoViewModelContext(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _dataContext.Refreshed += (s, e) => { if (e.HasFlag(DataContextDataAreaEnum.Knowledge)) MapToViewModels(); };

            Groups = new ObservableCollection<InfoGroupViewModel>();

            MapToViewModels();
        }

        public event EventHandler Invalidated;

        private void MapToViewModels()
        {
            Groups.Clear();
            
            var groups = _dataContext.InfoGroups.Select(entity => new InfoGroupViewModel(entity)).ToList();
            foreach (var group in groups)
            {
                foreach (var entry in group.Entity.Entries)
                {
                    group.Entries.Add(new InfoViewModel(entry));
                }
            }
            

            foreach(var viewModel in groups)
            {
                Groups.Add(viewModel);
            }

            Invalidated?.Invoke(this, null);
        }
    }
}
