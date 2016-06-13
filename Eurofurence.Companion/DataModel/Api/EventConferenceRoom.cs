using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Abstractions;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class EventConferenceRoom : EntityBase, ISortOrderKeyProvider
    {
        public EventConferenceRoom()
        {
            Entries = new Collection<EventEntry>();
        }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<EventEntry> Entries { get; set; }

        [JsonIgnore]
        public object SortOrderKey => Name;
    }
}