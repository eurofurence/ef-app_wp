using Windows.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class DealerViewModel : BindableBase
    {
        public Dealer Entity { get; }

        public List<Uri> ParsedWebsiteUris { get; set; }

        public MapEntryViewModel MapEntry { get; set; }
  
        public DealerViewModel(Dealer entity)
        {
            InitializeDispatcherFromCurrentThread();
            Entity = entity;
            ParsedWebsiteUris =
                entity.Links?
                    .Where(a => a.FragmentType == LinkFragment.FragmentTypeEnum.WebExternal)
                    .Select(a => new Uri(a.Target))
                    .ToList() 
                        ?? new List<Uri>();
        }

        public string DisplayName => HasUniqueDisplayName ? Entity.DisplayName : Entity.AttendeeNickname;
        public bool HasUniqueDisplayName => !string.IsNullOrWhiteSpace(Entity.DisplayName);
        public bool HasMapEntry => MapEntry != null;

        public bool HasArtistThumbnailImage => Entity.ArtistThumbnailImage != null;
        public bool HasArtistImage => Entity.ArtistImage != null;
        public bool HasArtPreviewImage => Entity.ArtPreviewImage != null;
        public bool HasArtPreviewCaption => !string.IsNullOrWhiteSpace(Entity.ArtPreviewCaption);

        public bool HasAboutTheArtistText => !string.IsNullOrWhiteSpace(Entity.AboutTheArtistText);
        public bool HasAboutTheArtText => !string.IsNullOrWhiteSpace(Entity.AboutTheArtText);
        public bool HasWebsiteUris => ParsedWebsiteUris.Count > 0;

        public bool HasTelegramHandle => !string.IsNullOrWhiteSpace(Entity.TelegramHandle);
        public bool HasTwitterHandle => !string.IsNullOrWhiteSpace(Entity.TwitterHandle);

        public bool AttendsOnAllDays => Entity.AttendsOnThursday && Entity.AttendsOnFriday && Entity.AttendsOnSaturday;

        private Color _dominantColor = Colors.Transparent;
        public Color DominantColor {  get { return _dominantColor; } set { SetProperty(ref _dominantColor, value); } }

        public bool IsAfterDark => Entity.IsAfterDark;
        public string[] Categories => Entity.Categories;
        public string CategoriesAsString => string.Join(", ", Entity.Categories);
        public bool HasCategories => Entity.Categories != null && Entity.Categories.Length > 0;

        public bool HasAvailabilityInformation => !AttendsOnAllDays || IsAfterDark || HasMapEntry;
    }
}