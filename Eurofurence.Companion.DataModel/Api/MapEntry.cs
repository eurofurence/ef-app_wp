using System;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class MapEntry : EntityBase
    {
        public Guid MapId { get; set; }
        public double RelativeX { get; set; }
        public double RelativeY { get; set; }
        public double RelativeTapRadius { get; set; }

        public string MarkerType  { get; set; }
        public Guid? TargetId { get; set; }
        public string TargetDescription { get; set; }

        [JsonIgnore]
        public Map Map { get; set; }

        [JsonIgnore]
        public EntityBase TargetEntity { get; set; }
    }
}