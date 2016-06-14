using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.DataStore.Abstractions
{
    public interface IDataStore
    {
        Task ApplyDeltaAsync(IEnumerable<EntityBase> entities, Action<int, int, string> progressCallback = null);
        Task<IList<T>> GetAsync<T>() where T : EntityBase, new();
        Task ClearAllAsync();
        Task ClearAsync(Type entityType);

        Task<Dictionary<string, ulong>> GetStorageFileSizesAsync();
    }
}