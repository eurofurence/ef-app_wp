using System;

namespace Eurofurence.Companion.DataModel
{
    public abstract class EntityBase : BindableBase
    {
        public Guid Id { get; set; }

        public byte IsDeleted { get; set; }
        public DateTime LastChangeDateTimeUtc { get; set; }

        protected EntityBase()
        {
            InitializeDispatcherFromCurrentThread();
        }
    }
}