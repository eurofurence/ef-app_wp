using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.DataStore
{
    public class RealtimeApiAccessDataStore : IDataStore
    {
        private readonly EurofurenceWebApiClient _apiClient;

        public RealtimeApiAccessDataStore()
        {
            _apiClient = new EurofurenceWebApiClient(Consts.WEB_API_ENDPOINT_URL);
        }

        public async Task ApplyDeltaAsync(IEnumerable<EntityBase> entities,
            Action<int, int, string> progressCallback = null)
        {
            await Task.Delay(1).ConfigureAwait(false);
        }

        public Task ClearAllAsync()
        {
            throw new InvalidOperationException();
        }

        public async Task<IList<T>> GetAsync<T>() where T : EntityBase, new()
        {
            var result = await _apiClient.GetEntitiesAsync<T>().ConfigureAwait(false);
            return result;
        }
    }
}