using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Abstractions;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class EventConferenceDay : EntityBase, ISortOrderKeyProvider, IEquatable<EventConferenceDay>
    {
        public EventConferenceDay()
        {
            Entries = new Collection<EventEntry>();
        }

        public string Name { get; set; }
        public DateTime Date { get; set; }


        [JsonIgnore]
        public ICollection<EventEntry> Entries { get; set; }

        [JsonIgnore]
        public string WeekdayAbbreviated => Date.ToString("ddd");

        [JsonIgnore]
        public string WeekdayFullname => Date.ToString("dddd");

        [JsonIgnore]
        public object SortOrderKey => Date.Ticks;

        [JsonIgnore]
        public DateTime DayStartDateTimeUtc => new DateTime((Date - TimeSpan.FromHours(2)).Ticks, DateTimeKind.Utc);

        [JsonIgnore]
        public DateTime DayEndDateTimeUtc => DayStartDateTimeUtc.AddHours(24);

        public bool Equals(EventConferenceDay other)
        {
            return Id == other.Id;
        }
    }
}