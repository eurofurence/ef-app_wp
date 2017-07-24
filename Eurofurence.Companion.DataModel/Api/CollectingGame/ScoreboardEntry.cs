namespace Eurofurence.Companion.DataModel.Api.CollectingGame
{
    public abstract class ScoreboardEntry
    {
        public int CollectionCount { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
    }
}