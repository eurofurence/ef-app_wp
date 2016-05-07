using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using System.Diagnostics;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Eurofurence.Companion.Common;

namespace Eurofurence.Companion.DataStore
{
    public class ContextManager : BindableBase
    {
        private string _status = "";

        private EurofurenceWebApiClient _apiClient;
        private IDataStore _dataStore;
        private IDataContext _dataContext;
        private ApplicationSettingsContext _applicationSettingsContext;

        private CoreDispatcher _dispatcher;

        private ulong _mainOperationMaxValue = 100;
        private ulong _mainOperationCurrentValue = 0;
        private ulong _subOperationMaxValue = 100;
        private ulong _subOperationCurrentValue = 0;

        public ulong SubOperationMaxValue { get { return _subOperationMaxValue; } set { SetProperty(ref _subOperationMaxValue, value); } }
        public ulong SubOperationCurrentValue { get { return _subOperationCurrentValue; } set { SetProperty(ref _subOperationCurrentValue, value); } }
        public ulong MainOperationMaxValue { get { return _mainOperationMaxValue; } set { SetProperty(ref _mainOperationMaxValue, value); } }
        public ulong MainOperationCurrentValue { get { return _mainOperationCurrentValue; } set { SetProperty(ref _mainOperationCurrentValue, value); } }


        public ICommand UpdateCommand { get; private set; }
        public ICommand ClearAllCommand { get; private set; }

        public string Status { get { return _status; } set { SetProperty(ref _status, value); } }
         
        public ContextManager(IDataStore dataStore, IDataContext dataContext, ApplicationSettingsContext applicationSettingsContext)
        {
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            _apiClient = new EurofurenceWebApiClient("http://eurofurencewebapi.azurewebsites.net");
            _dataStore = dataStore;
            _dataContext = dataContext;
            _applicationSettingsContext = applicationSettingsContext;

            Status = "Idle";
            UpdateCommand = new AwaitableCommand(Update);
            ClearAllCommand = new AwaitableCommand(ClearAll);
        }

        public async Task InitializeAsync()
        {
            await _dataContext.RefreshAsync();
        }

        public DateTime? LastServerQueryDateTimeUtc => _applicationSettingsContext.LastServerQueryDateTimeUtc; 

        public async Task ClearAll()
        {
            await _dataStore.ClearAllAsync();
            await _dataContext.RefreshAsync();

            _applicationSettingsContext.LastServerQueryDateTimeUtc = null;
            OnPropertyChanged(nameof(LastServerQueryDateTimeUtc));
        }

        public async Task Update()
        {
            Status = "Busy";

            var metadata = await _apiClient.GetEndpointMetadataAsync();

            var entities = new List<EntityBase>();

            MainOperationMaxValue = 9;
            MainOperationCurrentValue = 0;

            entities.AddRange(await UpdateEntitiesAsync<EventEntry>("Events", LastServerQueryDateTimeUtc));
            entities.AddRange(await UpdateEntitiesAsync<EventConferenceDay>("Event Conference Days", LastServerQueryDateTimeUtc));
            entities.AddRange(await UpdateEntitiesAsync<EventConferenceRoom>("Event Conference Rooms", LastServerQueryDateTimeUtc));
            entities.AddRange(await UpdateEntitiesAsync<EventConferenceTrack>("Event Conference Tracks", LastServerQueryDateTimeUtc));
            entities.AddRange(await UpdateEntitiesAsync<Info>("Info", LastServerQueryDateTimeUtc));
            entities.AddRange(await UpdateEntitiesAsync<InfoGroup>("Info Group", LastServerQueryDateTimeUtc));
            entities.AddRange(await UpdateEntitiesAsync<Image>("Images Metadata", LastServerQueryDateTimeUtc));

            await UpdateImageDataAsync(entities.Where(a => a is Image).Cast<Image>());

            Status = "Synchronizing";
            await _dataStore.ApplyDeltaAsync(entities, async (current, max, id) =>
            {
                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    SubOperationMaxValue = (ulong)max;
                    SubOperationCurrentValue = (ulong)current;
                });
            });

            _applicationSettingsContext.LastServerQueryDateTimeUtc = metadata.CurrentDateTimeUtc;
            OnPropertyChanged(nameof(LastServerQueryDateTimeUtc));

            Status = metadata.Configuration.First().Value;

            await _dataContext.RefreshAsync();
        }


        private async Task UpdateImageDataAsync(IEnumerable<Image> images)
        {
            var imageList = images.ToList();

            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { 
                Status = "Images";
                SubOperationMaxValue = (ulong)imageList.Count;
                SubOperationCurrentValue = 0;
            });

            foreach (var imageEntity in imageList)
            {
                var content = await _apiClient.GetContentAsync(imageEntity.Url.Replace("{Endpoint}", "http://eurofurencewebapi.azurewebsites.net"));
                var bytes = content.ToCharArray().Select(a => (byte)a).ToArray();

                imageEntity.Content = bytes;

                await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { SubOperationCurrentValue++; });
            }

            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { MainOperationCurrentValue++; });
        }

        private async Task<IList<T>> UpdateEntitiesAsync<T>(string entityName, DateTime? since)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Status = entityName; });

            var entities = await _apiClient.GetEntitiesAsync<T>(since);

            //, async (progress) =>
            //  {
            //      //await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //      //{
            //      //    SubOperationCurrentValue = progress.BytesReceived;
            //      //    SubOperationMaxValue = progress.TotalBytesToReceive ?? 0;
            //      //});
            //  })

            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { MainOperationCurrentValue++; });

            return entities;
        }


    }
}
