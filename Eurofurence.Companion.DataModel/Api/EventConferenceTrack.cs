using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Abstractions;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class EventConferenceTrack : EntityBase, ISortOrderKeyProvider, IEquatable<EventConferenceTrack>
    {
        public EventConferenceTrack()
        {
            Entries = new Collection<EventEntry>();
        }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<EventEntry> Entries { get; set; }

        [JsonIgnore]
        public object SortOrderKey => Name;

        public bool Equals(EventConferenceTrack other)
        {
            return Id == other.Id;
        }
    }
}