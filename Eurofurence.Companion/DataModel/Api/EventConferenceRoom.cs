using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eurofurence.Companion.ViewModel;
using SQLite;

namespace Eurofurence.Companion.DataModel.Api
{
    public class EventConferenceRoom : EntityBase, ISortOrderKeyProvider
    {
        public EventConferenceRoom()
        {
            Entries = new Collection<EventEntry>();
        }

        public string Name { get; set; }

        [Ignore]
        public ICollection<EventEntry> Entries { get; set; }

        [Ignore]
        public object SortOrderKey => Name;
    }
}