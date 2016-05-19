using Eurofurence.Companion.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Eurofurence.Companion.DataModel.Api
{


    public abstract class EntityBase
    {
        [SQLite.PrimaryKey]
        public Guid Id { get; set; }
        public byte IsDeleted { get; set; }
        public DateTime LastChangeDateTimeUtc { get; set; }
    }

    public class Endpoint
    {
        public DateTime CurrentDateTimeUtc { get; set; }
        public List<EndpointConfiguration> Configuration { get; set; }
    }

    public class EndpointConfiguration
    {
        public Guid Id { get; set; }
        public string ResourceKey { get; set; }
        public string Value { get; set; }
    }

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


        [SQLite.Ignore]
        public virtual EventConferenceTrack ConferenceTrack { get; set; }
        [SQLite.Ignore]
        public virtual EventConferenceDay ConferenceDay { get; set; }
        [SQLite.Ignore]
        public virtual EventConferenceRoom ConferenceRoom { get; set; }
        [SQLite.Ignore]
        public virtual Image Image { get; set; }
        [SQLite.Ignore]
        public object SortOrderKey => (ConferenceDay?.Date.Ticks ?? 0) + StartTime.TotalSeconds;
    }

    public class EventConferenceDay : EntityBase, ISortOrderKeyProvider
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        [SQLite.Ignore]
        public virtual ICollection<EventEntry> Entries { get; set; }
        [SQLite.Ignore]
        public object SortOrderKey => Date.Ticks;
        [SQLite.Ignore]
        public string WeekdayAbbreviated => this.Date.ToString("ddd");
        [SQLite.Ignore]
        public string WeekdayFullname => this.Date.ToString("dddd");

        public EventConferenceDay()
        {
            Entries = new Collection<EventEntry>();
        }
    }

    public class EventConferenceRoom : EntityBase, ISortOrderKeyProvider
    {
        public string Name { get; set; }

        [SQLite.Ignore]
        public ICollection<EventEntry> Entries { get; set; }

        [SQLite.Ignore]
        public object SortOrderKey => Name;

        public EventConferenceRoom()
        {
            Entries = new Collection<EventEntry>();
        }
    }

    public class EventConferenceTrack : EntityBase, ISortOrderKeyProvider
    {
        public string Name { get; set; }

        [SQLite.Ignore]
        public virtual ICollection<EventEntry> Entries { get; set; }

        [SQLite.Ignore]
        public object SortOrderKey => Name;

        public EventConferenceTrack()
        {
            Entries = new Collection<EventEntry>();
        }
    }

    public class Info : EntityBase
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Position { get; set; }

        public Guid InfoGroupId { get; set; }

        [SQLite.Ignore]
        public virtual InfoGroup Group { get; set; }
    }

    public class InfoGroup : EntityBase, ISortOrderKeyProvider
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }

        [SQLite.Ignore]
        public ICollection<Info> Entries { get; set; }

        [SQLite.Ignore]
        public object SortOrderKey => Position;

        public InfoGroup()
        {
            Entries = new Collection<Info>();
        }
    }

    public class Dealer: EntityBase, ISortOrderKeyProvider
    {
        public int RegistrationNumber { get; set; }
        public string AttendeeNickname { get; set; }
        public string DisplayName { get; set; }
        public string ShortDescription { get; set; }
        public string AboutTheArtistText { get; set; }
        public string AboutTheArtText { get; set; }
        public string WebsiteUri { get; set; }
        public string ArtPreviewCaption { get; set; }
        public Guid? ArtistThumbnailImageId { get; set; }
        public Guid? ArtistImageId { get; set; }
        public Guid? ArtPreviewImageId { get; set; }

        [SQLite.Ignore]
        public string UIDisplayName => !string.IsNullOrWhiteSpace(DisplayName) ? DisplayName : AttendeeNickname;

        [SQLite.Ignore]
        public object SortOrderKey => UIDisplayName;

        [SQLite.Ignore]
        public virtual Image ArtistThumbnailImage { get; set; }
        [SQLite.Ignore]
        public virtual Image ArtistImage { get; set; }
        [SQLite.Ignore]
        public virtual Image ArtPreviewImage { get; set; }

    }

    public class Image : EntityBase
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int FileSizeInBytes { get; set; }
        public string MimeType{ get; set; }
        public byte[] Content { get; set; }
        
    }


}
