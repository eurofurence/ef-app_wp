using System;
using Eurofurence.Companion.ViewModel;
using SQLite;

namespace Eurofurence.Companion.DataModel.Api
{
    public class EventEntry : EntityBase, ISortOrderKeyProvider
    {
        public int SourceEventId { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
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


        [Ignore]
        public DateTime? StartTimeAndDay
            => ConferenceDay == null ? (DateTime?)null : (ConferenceDay.Date + StartTime);

        [Ignore]
        public virtual EventConferenceTrack ConferenceTrack { get; set; }

        [Ignore]
        public virtual EventConferenceDay ConferenceDay { get; set; }

        [Ignore]
        public virtual EventConferenceRoom ConferenceRoom { get; set; }

        [Ignore]
        public virtual Image Image { get; set; }

        [Ignore]
        public object SortOrderKey => (ConferenceDay?.Date.Ticks ?? 0) + StartTime.TotalSeconds;
    }
}