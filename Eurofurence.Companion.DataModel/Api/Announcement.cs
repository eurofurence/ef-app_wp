using System;
using Eurofurence.Companion.DataModel.Abstractions;
using Newtonsoft.Json;

namespace Eurofurence.Companion.DataModel.Api
{
    public class Announcement : EntityBase, ISortOrderKeyProvider
    {
        public DateTime ValidFromDateTimeUtc { get; set; }
        public DateTime ValidUntilDateTimeUtc { get; set; }

        public string Area { get; set; }
        public string Author{ get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [JsonIgnore]
        public object SortOrderKey => ValidFromDateTimeUtc;
    }
}