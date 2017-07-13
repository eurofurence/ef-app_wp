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
    [IocBeacon(TargetType = typeof(IKnowledgeViewModelContext), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class KnowledgeViewModelContext : BindableBase, IKnowledgeViewModelContext
    {
        private IDataContext _dataContext;

        public ObservableCollection<KnowledgeGroupViewModel> Groups { get; }

        public KnowledgeViewModelContext(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _dataContext.Refreshed += (s, e) => { if (e.HasFlag(DataContextDataAreaEnum.Knowledge)) MapToViewModels(); };

            Groups = new ObservableCollection<KnowledgeGroupViewModel>();

            MapToViewModels();
        }

        public event EventHandler Invalidated;

        private void MapToViewModels()
        {
            Groups.Clear();
            
            var groups = _dataContext.KnowledgeGroups.Select(entity => new KnowledgeGroupViewModel(entity)).ToList();

            foreach (var group in groups)
            {
                foreach (var entry in group.Entity.Entries)
                {
                    var vm = new KnowledgeEntryViewModel(entry);
                    vm.LinkActions = LinkFragmentActionFactory.ConvertFragments(entry.Links);

                    group.Entries.Add(vm);
                }

                group.Resolve();
            }
            

            foreach(var viewModel in groups)
            {
                Groups.Add(viewModel);
            }

            Invalidated?.Invoke(this, null);
        }
    }
}
