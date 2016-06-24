using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class Map : EntityBase
    {
        public Guid ImageId { get; set; }
        public string Description { get; set; }
        public int IsBrowseable { get; set; }
        [JsonIgnore]
        public Image Image { get; set; }
        
        [JsonIgnore]
        public List<MapEntry>  Entries { get; set; }

        public Map()
        {
            Entries = new List<MapEntry>();
        }
    }
}