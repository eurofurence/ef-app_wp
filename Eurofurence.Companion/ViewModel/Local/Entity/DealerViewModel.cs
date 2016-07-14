using Windows.UI;
using System.Threading.Tasks;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

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
            ParsedWebsiteUris = new List<Uri>();

            ParseWebSiteUris();

            CalculateDominantColorAsync();
        }

        private void ParseWebSiteUris()
        {
            if (string.IsNullOrWhiteSpace(Entity.WebsiteUri)) return;

            var sanitizedParts = Entity.WebsiteUri
                .Replace(" / ", ";")
                .Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var part in sanitizedParts)
            {
                var assumedUri = part;
                if (!assumedUri.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) && 
                    !assumedUri.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))
                {
                    assumedUri = $"http://{part}";
                }

                if (Uri.IsWellFormedUriString(assumedUri, UriKind.Absolute))
                {
                    ParsedWebsiteUris.Add(new Uri(assumedUri, UriKind.Absolute));
                }
            }
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

        private Color _dominantColor = Colors.Transparent;
        public Color DominantColor {  get { return _dominantColor; } set { SetProperty(ref _dominantColor, value); } }

        public async Task CalculateDominantColorAsync()
        {
            if (!HasArtistImage) return;
            //DominantColor = await Entity.ArtistImage.GetDominantColorAsync();
        }
    }
}