using System;

namespace Eurofurence.Companion.DataModel
{
    public interface IEntityExtension
    {
        Guid Id { get; set; }
        bool IsPersistent { get; } 
    }
}
