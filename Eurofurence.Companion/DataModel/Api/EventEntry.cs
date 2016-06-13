using System;
using Eurofurence.Companion.DataModel.Abstractions;
using Eurofurence.Companion.DataModel.Local;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class EventEntry : EntityBase, ISortOrderKeyProvider
    {
        public int SourceEventId { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public Guid ConferenceTrackId { get; set; }
        public string Abstract { get; set; }
        public string Description { get; set; }
        public Guid ConferenceDayId { get; set; }
        public Guid? ImageId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public Guid ConferenceRoomId { get; set; }
        public string PanelHosts { get; set; }
        public byte IsDeviatingFromConBook { get; set; }


        [JsonIgnore]
        public ExtensionProxy<EventEntry, EventEntryAttributes> AttributesProxy { get; set; }

        [JsonIgnore]
        public DateTime EventDateTimeUtc
            => ConferenceDay == null
                ? DateTime.MinValue
                : new DateTime((ConferenceDay.Date + StartTime - TimeSpan.FromHours(2)).Ticks, DateTimeKind.Utc);

        [JsonIgnore]
        public virtual EventConferenceTrack ConferenceTrack { get; set; }

        [JsonIgnore]
        public virtual EventConferenceDay ConferenceDay { get; set; }

        [JsonIgnore]
        public virtual EventConferenceRoom ConferenceRoom { get; set; }

        [JsonIgnore]
        public virtual Image Image { get; set; }

        [JsonIgnore]
        public object SortOrderKey => (ConferenceDay?.Date.Ticks ?? 0) + StartTime.TotalSeconds;
    }
}