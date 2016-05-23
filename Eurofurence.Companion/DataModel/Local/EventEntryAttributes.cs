using System;

namespace Eurofurence.Companion.DataModel.Local
{
    public class EventEntryAttributes : EntityBase, IEntityExtension
    {
        public Guid Id { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsPersistent
        {
            get
            {
                return IsFavorite != false;
            }
        }
    }
}
