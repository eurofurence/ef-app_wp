using System;

namespace Eurofurence.Companion.DataModel.Api.CollectingGame
{
    public class PlayerCollectionEntry
    {
        public Guid BadgeId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Gender { get; set; }
        public DateTime CollectedAtDateTimeUtc { get; set; }
        public int CollectionCount { get; set; }
    }
}