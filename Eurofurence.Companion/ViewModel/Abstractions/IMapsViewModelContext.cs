using System;
using System.Collections.ObjectModel;
using Eurofurence.Companion.ViewModel.Local.Entity;

namespace Eurofurence.Companion.ViewModel.Abstractions
{
    public interface IMapsViewModelContext
    {
        ObservableCollection<MapViewModel> Maps { get; }
        event EventHandler Invalidated;
    }
}