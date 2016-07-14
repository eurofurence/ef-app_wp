using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;
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

        Task SaveBlobAsync(Guid id, string prefix, byte[] content);
        Task ClearBlobAsync(Guid id, string prefix);
        Task<byte[]> GetBlobAsync(Guid id, string prefix);
        Task<IRandomAccessStream> GetBlobStreamAsync(Guid id, string prefix);

        Task ClearAllBlobsAsync();

    }
}