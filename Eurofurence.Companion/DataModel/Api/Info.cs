using System;
using SQLite;

namespace Eurofurence.Companion.DataModel.Api
{
    public class Info : EntityBase
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Position { get; set; }

        public Guid InfoGroupId { get; set; }

        [Ignore]
        public virtual InfoGroup Group { get; set; }
    }
}