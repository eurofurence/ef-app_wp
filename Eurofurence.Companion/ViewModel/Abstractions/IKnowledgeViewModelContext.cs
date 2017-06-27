using System;
using System.Collections.ObjectModel;
using Eurofurence.Companion.ViewModel.Local.Entity;

namespace Eurofurence.Companion.ViewModel.Abstractions
{
    public interface IKnowledgeViewModelContext
    {
        ObservableCollection<KnowledgeGroupViewModel> Groups { get; }

        event EventHandler Invalidated;
    }
}