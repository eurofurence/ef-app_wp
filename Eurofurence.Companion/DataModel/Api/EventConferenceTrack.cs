using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Abstractions;
using Eurofurence.Companion.ViewModel;
using SQLite;

namespace Eurofurence.Companion.DataModel.Api
{
    public class EventConferenceTrack : EntityBase, ISortOrderKeyProvider
    {
        public EventConferenceTrack()
        {
            Entries = new Collection<EventEntry>();
        }

        public string Name { get; set; }

        [Ignore]
        public virtual ICollection<EventEntry> Entries { get; set; }

        [Ignore]
        public object SortOrderKey => Name;
    }
}