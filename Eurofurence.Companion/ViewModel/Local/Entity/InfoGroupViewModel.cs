using Eurofurence.Companion.DataModel.Api;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class InfoGroupViewModel : BindableBase
    {
        public InfoGroup Entity { get; }
        public ICollection<InfoViewModel> Entries { get; }

        public InfoGroupViewModel(InfoGroup entity)
        {
            Entity = entity;
            Entries = new Collection<InfoViewModel>();
        }
    }
}
