using System;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class MapEntry : EntityBase
    {
        public Guid MapId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int TapRadius { get; set; }

        public LinkFragment[] Links { get; set; }


        [JsonIgnore]
        public Map Map { get; set; }

        [JsonIgnore]
        public EntityBase TargetEntity { get; set; }
    }
}