using System;
using System.Collections.Generic;
using Eurofurence.Companion.DataModel.Abstractions;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class Info : EntityBase, ISortOrderKeyProvider
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Position { get; set; }

        public Guid InfoGroupId { get; set; }

        public List<Guid> ImageIds { get; set; }
        public List<NamedUrl> Urls { get; set; }

        [JsonIgnore]
        public virtual InfoGroup Group { get; set; }

        [JsonIgnore]
        public object SortOrderKey => Position;
    }
}