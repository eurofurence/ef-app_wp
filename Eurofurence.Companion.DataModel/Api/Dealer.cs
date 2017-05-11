using System;
using Eurofurence.Companion.DataModel.Abstractions;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class Dealer : EntityBase, ISortOrderKeyProvider
    {
        public int RegistrationNumber { get; set; }
        public string AttendeeNickname { get; set; }
        public string DisplayName { get; set; }
        public string Merchandise { get; set; }
        public string ShortDescription { get; set; }
        public string AboutTheArtistText { get; set; }
        public string AboutTheArtText { get; set; }
        public LinkFragment[] Links { get; set; }
        public string ArtPreviewCaption { get; set; }
        public Guid? ArtistThumbnailImageId { get; set; }
        public Guid? ArtistImageId { get; set; }
        public Guid? ArtPreviewImageId { get; set; }
        public bool AttendsOnThursday { get; set; }
        public bool AttendsOnFriday { get; set; }
        public bool AttendsOnSaturday { get; set; }
        public string TwitterHandle { get; set; }
        public string TelegramHandle { get; set; }

        [JsonIgnore]
        public virtual Image ArtistThumbnailImage { get; set; }

        [JsonIgnore]
        public virtual Image ArtistImage { get; set; }

        [JsonIgnore]
        public virtual Image ArtPreviewImage { get; set; }

        [JsonIgnore]
        public object SortOrderKey => DisplayName ?? AttendeeNickname;
    }
}