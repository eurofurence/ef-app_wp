using System;
using SQLite;
using Eurofurence.Companion.DataModel.Abstractions;

namespace Eurofurence.Companion.DataModel.Api
{
    public class Info : EntityBase, ISortOrderKeyProvider
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Position { get; set; }

        public Guid InfoGroupId { get; set; }

        [Ignore]
        public virtual InfoGroup Group { get; set; }

        [Ignore]
        public object SortOrderKey => Position;
    }
}