using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Abstractions;
using SQLite;

namespace Eurofurence.Companion.DataModel.Api
{
    public class InfoGroup : EntityBase, ISortOrderKeyProvider
    {
        public InfoGroup()
        {
            Entries = new Collection<Info>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }

        [Ignore]
        public ICollection<Info> Entries { get; set; }

        [Ignore]
        public object SortOrderKey => Position;
    }
}