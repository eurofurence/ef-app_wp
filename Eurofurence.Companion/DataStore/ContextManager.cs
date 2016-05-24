using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.DataStore
{
    [IocBeacon(TargetType = typeof(ContextManager), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class ContextManager : BindableBase
    {
        private readonly EurofurenceWebApiClient _apiClient;
        private readonly ApplicationSettingsContext _applicationSettingsContext;
        private readonly IDataContext _dataContext;
        private readonly IDataStore _dataStore;

        private ulong _mainOperationCurrentValue;
        private ulong _mainOperationMaxValue = 100;

        private string _mainOperationMessage = "";
        private ulong _subOperationCurrentValue;
        private ulong _subOperationMaxValue = 100;

        private TaskStatus _updateStatus = TaskStatus.WaitingForActivation;


        public ContextManager(IDataStore dataStore, IDataContext dataContext,
            ApplicationSettingsContext applicationSettingsContext)
        {
            InitializeDispatcherFromCurrentThread();

            _apiClient = new EurofurenceWebApiClient(Consts.WEB_API_ENDPOINT_URL);
            _dataStore = dataStore;
            _dataContext = dataContext;
            _applicationSettingsContext = applicationSettingsContext;

            MainOperationMessage = "";

            UpdateStatus = _applicationSettingsContext.LastServerQueryDateTimeUtc.HasValue
                ? TaskStatus.RanToCompletion
                : TaskStatus.WaitingToRun;

            UpdateCommand = new AwaitableCommand(Update);
            ClearAllCommand = new AwaitableCommand(ClearAll);
        }

        public ulong SubOperationMaxValue
        {
            get { return _subOperationMaxValue; }
            set { SetProperty(ref _subOperationMaxValue, value); }
        }

        public ulong SubOperationCurrentValue
        {
            get { return _subOperationCurrentValue; }
            set { SetProperty(ref _subOperationCurrentValue, value); }
        }

        public ulong MainOperationMaxValue
        {
            get { return _mainOperationMaxValue; }
            set { SetProperty(ref _mainOperationMaxValue, value); }
        }

        public ulong MainOperationCurrentValue
        {
            get { return _mainOperationCurrentValue; }
            set { SetProperty(ref _mainOperationCurrentValue, value); }
        }

        public ICommand UpdateCommand { get; private set; }
        public ICommand ClearAllCommand { get; private set; }

        public string MainOperationMessage
        {
            get { return _mainOperationMessage; }
            set { SetProperty(ref _mainOperationMessage, value); }
        }

        public TaskStatus UpdateStatus
        {
            get { return _updateStatus; }
            set { SetProperty(ref _updateStatus, value); }
        }

        public DateTime? LastServerQueryDateTimeUtc => _applicationSettingsContext.LastServerQueryDateTimeUtc;

        public async Task InitializeAsync()
        {
            await _dataContext.RefreshAsync();
        }

        public async Task ClearAll()
        {
            await _dataStore.ClearAllAsync();
            await _dataContext.RefreshAsync();

            _applicationSettingsContext.LastServerQueryDateTimeUtc = null;
            OnPropertyChanged(nameof(LastServerQueryDateTimeUtc));
        }

        public async Task Update()
        {
            UpdateStatus = TaskStatus.Running;
            MainOperationMessage = "Initializing...";

            await _dataContext.SaveAsync().ConfigureAwait(false);

            var metadata = await _apiClient.GetEndpointMetadataAsync();

            var entities = new List<EntityBase>();

            MainOperationMaxValue = 9;
            MainOperationCurrentValue = 0;

            entities.AddRange(
                await UpdateEntitiesAsync<EventEntry>("Events", LastServerQueryDateTimeUtc));
            entities.AddRange(
                await UpdateEntitiesAsync<EventConferenceDay>("Event Conference Days", LastServerQueryDateTimeUtc));
            entities.AddRange(
                await UpdateEntitiesAsync<EventConferenceRoom>("Event Conference Rooms", LastServerQueryDateTimeUtc));
            entities.AddRange(
                await UpdateEntitiesAsync<EventConferenceTrack>("Event Conference Tracks", LastServerQueryDateTimeUtc));
            entities.AddRange(
                await UpdateEntitiesAsync<Info>("Info", LastServerQueryDateTimeUtc));
            entities.AddRange(
                await UpdateEntitiesAsync<InfoGroup>("Info Group", LastServerQueryDateTimeUtc));
            entities.AddRange(
                await UpdateEntitiesAsync<Image>("Images Metadata", LastServerQueryDateTimeUtc));
            entities.AddRange(
                await UpdateEntitiesAsync<Dealer>("Dealers", LastServerQueryDateTimeUtc));

            await UpdateImageDataAsync(entities.Where(a => a is Image).Cast<Image>());

            MainOperationMessage = "Synchronizing...";
            await _dataStore.ApplyDeltaAsync(entities, async (current, max, id) =>
            {
                SubOperationMaxValue = (ulong) max;
                SubOperationCurrentValue = (ulong) current;
            });

            _applicationSettingsContext.LastServerQueryDateTimeUtc = metadata.CurrentDateTimeUtc;
            OnPropertyChanged(nameof(LastServerQueryDateTimeUtc));

            await _dataContext.RefreshAsync();

            MainOperationMessage = "Done!";
            UpdateStatus = TaskStatus.RanToCompletion;
        }


        private async Task UpdateImageDataAsync(IEnumerable<Image> images)
        {
            var imageList = images.ToList();

            MainOperationMessage = "Downloading Image Content...";
            SubOperationMaxValue = (ulong) imageList.Count;
            SubOperationCurrentValue = 0;

            var tasks = new List<Task>();

            foreach (var imageEntity in imageList)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var content =
                        await
                            _apiClient.GetContentAsBufferAsync(imageEntity.Url.Replace("{Endpoint}",
                                Consts.WEB_API_ENDPOINT_URL)).ConfigureAwait(false);
                    var bytes = content.ToArray();

                    imageEntity.Content = bytes;

                    SubOperationCurrentValue++;
                }));
            }

            foreach(var task in tasks)
            {
                await task.ConfigureAwait(false);
            }

            MainOperationCurrentValue++;
        }

        private async Task<IList<T>> UpdateEntitiesAsync<T>(string entityName, DateTime? since)
        {
            MainOperationMessage = $"Downloading {entityName}..."; 
            var entities = await _apiClient.GetEntitiesAsync<T>(since);

            MainOperationCurrentValue++;
            return entities;
        }
    }
}