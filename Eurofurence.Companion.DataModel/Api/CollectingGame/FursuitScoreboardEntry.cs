using System;

namespace Eurofurence.Companion.DataModel.Api.CollectingGame
{
    public class FursuitScoreboardEntry : ScoreboardEntry
    {
        public Guid BadgeId { get; set; }
        public string Gender { get; set; }
        public string Species { get; set; }
    }
}