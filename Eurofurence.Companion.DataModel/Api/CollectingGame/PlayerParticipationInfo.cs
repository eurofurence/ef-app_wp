using System;

namespace Eurofurence.Companion.DataModel.Api.CollectingGame
{
    public class PlayerParticipationInfo
    {
        public string Name { get; set; }
        public int CollectionCount { get; set; }
        public int? ScoreboardRank { get; set; }

        public BadgeInfo[] RecentlyCollected { get; set; }

        public class BadgeInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public virtual bool HasRecentlyCollected => RecentlyCollected.Length > 0;
    }
}