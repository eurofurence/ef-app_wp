using System;
using SQLite;
using Eurofurence.Companion.DataModel.Abstractions;

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

        [Ignore]
        public object SortOrderKey => ValidFromDateTimeUtc;
    }
}