using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.DataStore
{
    public class RealtimeApiAccessDataStore : IDataStore
    {
        private EurofurenceWebApiClient _apiClient;

        public RealtimeApiAccessDataStore()
        {
            _apiClient = new EurofurenceWebApiClient("https://eurofurencewebapi.azurewebsites.net/");
        }

        public async Task ApplyDeltaAsync(IEnumerable<EntityBase> entities, Action<int, int, string> progressCallback = null)
        {
            await Task.Delay(1).ConfigureAwait(false);
        }

        public Task ClearAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IList<T>> GetAsync<T>() where T : EntityBase, new()
        {
            var result = await _apiClient.GetEntitiesAsync<T>().ConfigureAwait(false);
            return result;
        }
    }
}
