using System;

namespace Eurofurence.Companion.DataModel.Api.CollectingGame
{
    public class PlayerParticipationInfo
    {
        public int CollectionCount { get; set; }
        public int? ScoreboardRank { get; set; }

    }

    public class CollectTokenResponse
    {
        public bool IsSuccessful { get; set; }
        public string FailureMessage { get; set; }

        public Guid? FursuitBadgeId { get; set; }

        public int CollectionCount { get; set; }

        public string Name { get; set; }
        public string Species { get; set; }
        public string Gender { get; set; }
    }
}