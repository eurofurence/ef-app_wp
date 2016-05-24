using System;
using SQLite;

namespace Eurofurence.Companion.DataModel
{
    public abstract class EntityBase : BindableBase
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public byte IsDeleted { get; set; }
        public DateTime LastChangeDateTimeUtc { get; set; }
    }
}