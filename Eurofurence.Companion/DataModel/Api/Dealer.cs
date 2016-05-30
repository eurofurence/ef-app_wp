using System;
using Eurofurence.Companion.DataModel.Abstractions;
using Eurofurence.Companion.ViewModel;
using SQLite;

namespace Eurofurence.Companion.DataModel.Api
{
    public class Dealer : EntityBase, ISortOrderKeyProvider
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

        [Ignore]
        public string UiDisplayName => !string.IsNullOrWhiteSpace(DisplayName) ? DisplayName : AttendeeNickname;

        [Ignore]
        public virtual Image ArtistThumbnailImage { get; set; }

        [Ignore]
        public virtual Image ArtistImage { get; set; }

        [Ignore]
        public virtual Image ArtPreviewImage { get; set; }

        [Ignore]
        public object SortOrderKey => UiDisplayName;
    }
}