using System;
using System.Collections.ObjectModel;
using Eurofurence.Companion.ViewModel.Local.Entity;

namespace Eurofurence.Companion.ViewModel.Abstractions
{
    public interface IInfoViewModelContext
    {
        ObservableCollection<InfoGroupViewModel> Groups { get; }

        event EventHandler Invalidated;
    }
}