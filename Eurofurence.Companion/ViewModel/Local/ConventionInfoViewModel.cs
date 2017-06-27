using System.Collections.ObjectModel;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon]
    public class ConventionInfoViewModel : BindableBase
    {
        private IKnowledgeViewModelContext _knowledgeViewModelContext;

        public ObservableCollection<KnowledgeGroupViewModel> Groups => _knowledgeViewModelContext.Groups;


        public ConventionInfoViewModel(IKnowledgeViewModelContext knowledgeViewModelContext)
        {
            _knowledgeViewModelContext = knowledgeViewModelContext;
            _knowledgeViewModelContext.Invalidated += (s, e) => { OnPropertyChanged(nameof(Groups)); };
        }
    }
}
