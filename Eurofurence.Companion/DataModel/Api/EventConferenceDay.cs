using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Abstractions;
using SQLite;

namespace Eurofurence.Companion.DataModel.Api
{
    public class EventConferenceDay : EntityBase, ISortOrderKeyProvider
    {
        public EventConferenceDay()
        {
            Entries = new Collection<EventEntry>();
        }

        public string Name { get; set; }
        public DateTime Date { get; set; }

        [Ignore]
        public ICollection<EventEntry> Entries { get; set; }

        [Ignore]
        public string WeekdayAbbreviated => Date.ToString("ddd");

        [Ignore]
        public string WeekdayFullname => Date.ToString("dddd");

        [Ignore]
        public object SortOrderKey => Date.Ticks;
    }
}