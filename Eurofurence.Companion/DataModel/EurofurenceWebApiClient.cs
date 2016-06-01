using Eurofurence.Companion.DataModel.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public EurofurenceWebApiClient(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
        }

        public Task<Endpoint> GetEndpointMetadataAsync()
        {
            return GetAsync<Endpoint>("Endpoint");
        }

        private string GetResourcePath(Type type)
        {
            switch (type.Name)
            {
                case "EventEntry":
                case "EventConferenceDay":
                case "EventConferenceRoom":
                case "EventConferenceTrack":
                case "Info":
                case "InfoGroup":
                case "Image":
                case "Dealer":
                    return type.Name;
            }

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
