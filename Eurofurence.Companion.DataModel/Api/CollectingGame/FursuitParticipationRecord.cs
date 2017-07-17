using System;
using System.Collections.Generic;

namespace Eurofurence.Companion.DataModel.Api.CollectingGame
{
    public class FursuitParticipationRecord : EntityBase
    {
        public class CollectionEntry
        {
            public string PlayerParticipationUid { get; set; }
            public DateTime EventDateTimeUtc { get; set; }
        }

        public string OwnerUid { get; set; }
        public Guid FursuitBadgeId { get; set; }

        public string TokenValue { get; set; }
        public bool IsBanned { get; set; }

        public DateTime TokenRegistrationDateTimeUtc { get; set; }

        public int CollectionCount { get; set; }
        public IList<CollectionEntry> CollectionEntries { get; set; }

        public FursuitParticipationRecord()
        {
            CollectionEntries = new List<CollectionEntry>();
        }
    }
}