using Eurofurence.Companion.DataModel.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Eurofurence.Companion.DataModel
{
    public class EurofurenceWebApiClient
    {
        private readonly string _endpointUrl;

        private static readonly Dictionary<Type, string> _resourcePathMap = new Dictionary<Type, string>()
        {
            { typeof(EventEntry), "EventEntry" },
            { typeof(EventConferenceDay), "EventConferenceDay" },
            { typeof(EventConferenceRoom), "EventConferenceRoom" },
            { typeof(EventConferenceTrack), "EventConferenceTrack" },
            { typeof(Info), "Info" },
            { typeof(InfoGroup), "InfoGroup" },
            { typeof(Image), "Image" },
            { typeof(Dealer), "Dealer" },
        };
     
        public EurofurenceWebApiClient(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
        }

        public Task<Endpoint> GetEndpointMetadataAsync()
        {
            return GetAsync<Endpoint>("Endpoint");
        }

        public Type GetTypeForEntity(string name)
        {
            if (_resourcePathMap.ContainsValue(name))
                return _resourcePathMap.Single(a => a.Value == name).Key;

            return null;
        }

        private string GetResourcePath(Type type)
        {
            if (_resourcePathMap.ContainsKey(type)) return _resourcePathMap[type];

            throw new NotSupportedException();
        }

        public Task<List<T>> GetEntitiesAsync<T>(DateTime? since = null, Action<HttpProgress> progressCallback = null)
        {
            var uri = GetResourcePath(typeof(T));

            if (since.HasValue)
            {
                uri = $"{uri}?since={since.Value.ToString("yyyy-MM-ddTHH:mm:ssZ")}";
            } 

            return GetAsync<List<T>>(uri, progressCallback);
        }

        private async Task<T> GetResponseAsync<T>(Func<HttpResponseMessage, Task<T>> selector,  string url, Action<HttpProgress> progressCallback = null)
        {
            var filter = new HttpBaseProtocolFilter {AutomaticDecompression = true};
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            filter.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);

            using (var client = new HttpClient(filter))
            {
                client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                var responseOperation = client.GetAsync(new Uri(url));

                if (progressCallback != null)
                {
                    responseOperation.Progress = (res, progress) =>
                    {
                        progressCallback(progress);
                    };
                }

                var response = await responseOperation;
                response.EnsureSuccessStatusCode();


                T result = await selector.Invoke(response);
                return result;
            }
        }

        public Task<string> GetContentAsStringAsync(string url, Action<HttpProgress> progressCallback = null)
        {
            return GetResponseAsync(async response => await response.Content.ReadAsStringAsync(), url, progressCallback);
        }

        public Task<IBuffer> GetContentAsBufferAsync(string url, Action<HttpProgress> progressCallback = null)
        {
            return GetResponseAsync(async response => await response.Content.ReadAsBufferAsync(), url, progressCallback);
        }


        private async Task<T> GetAsync<T>(string resource, Action<HttpProgress> progressCallback = null)
        {
            var url = $"{_endpointUrl}/{resource}";
            var content = await GetContentAsStringAsync(url, progressCallback).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
        }


    }
}
