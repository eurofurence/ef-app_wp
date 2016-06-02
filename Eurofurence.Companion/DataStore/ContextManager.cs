using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;

// ReSharper disable ExplicitCallerInfoArgument

namespace Eurofurence.Companion.DataStore
{
    [IocBeacon(TargetType = typeof (ContextManager), Scope = IocBeacon.ScopeEnum.Singleton)]
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

        private class EntityUpdateResult
        {
            public Type EntityType { get; set; }
            public bool TruncateBeforeProcessing { get; set; }
            public List<EntityBase> Entities { get; set; }
        }

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
            MainOperationMessage = $"{Translations.ContextManager_Update_Initializing}...";

            await _dataContext.SaveAsync();

            var metadata = await _apiClient.GetEndpointMetadataAsync();
            var updateResults = new List<EntityUpdateResult>();

            MainOperationMaxValue = 9;
            MainOperationCurrentValue = 0;

            updateResults.Add(await UpdateEntitiesAsync<EventEntry>(metadata));
            updateResults.Add(await UpdateEntitiesAsync<EventConferenceDay>(metadata));
            updateResults.Add(await UpdateEntitiesAsync<EventConferenceRoom>(metadata));
            updateResults.Add(await UpdateEntitiesAsync<EventConferenceTrack>(metadata));
            updateResults.Add(await UpdateEntitiesAsync<Info>(metadata));
            updateResults.Add(await UpdateEntitiesAsync<InfoGroup>(metadata));
            updateResults.Add(await UpdateEntitiesAsync<Image>(metadata));
            updateResults.Add(await UpdateEntitiesAsync<Dealer>(metadata));

            await UpdateImageDataAsync(updateResults.Single(a => a.EntityType == typeof (Image)).Entities.Cast<Image>());

            MainOperationMessage = Translations.ContextManager_Update_Initializing;

            foreach (var entity in updateResults.Where(a => a.TruncateBeforeProcessing).Select(b => b.EntityType))
            {
                await _dataStore.ClearAsync(entity);
            }

            await _dataStore.ApplyDeltaAsync(updateResults.SelectMany(a => a.Entities), (current, max, id) =>
            {
                SubOperationMaxValue = (ulong) max;
                SubOperationCurrentValue = (ulong) current;
            });

            _applicationSettingsContext.LastServerQueryDateTimeUtc = metadata.CurrentDateTimeUtc;
            OnPropertyChanged(nameof(LastServerQueryDateTimeUtc));

            await _dataContext.RefreshAsync();
            await _dataContext.SaveAsync();

            MainOperationMessage = $"{Translations.ContextManager_Update_Done}!";
            UpdateStatus = TaskStatus.RanToCompletion;
        }


        private async Task UpdateImageDataAsync(IEnumerable<Image> images)
        {
            var imageList = images.ToList();

            MainOperationMessage = $"{Translations.ContextManager_Update_DownloadingImageContent}...";

            SubOperationMaxValue = (ulong) imageList.Count;
            SubOperationCurrentValue = 0;

            var tasks = imageList.Select(imageEntity => Task.Run(async () =>
            {
                var content =
                    await
                        _apiClient.GetContentAsBufferAsync(
                            imageEntity.Url
                                .Replace("{Endpoint}", Consts.WEB_API_ENDPOINT_URL)
                                .Replace("{EndpointUrl}", Consts.WEB_API_ENDPOINT_URL)
                            ).ConfigureAwait(false);
                var bytes = content.ToArray();

                imageEntity.Content = bytes;

                SubOperationCurrentValue++;
            })).ToList();

            foreach (var task in tasks)
            {
                await task.ConfigureAwait(false);
            }

            MainOperationCurrentValue++;
        }

        private async Task<EntityUpdateResult> UpdateEntitiesAsync<T>(Endpoint metadata) where T : EntityBase
        {
            var result = new EntityUpdateResult {EntityType = typeof (T), Entities = new List<EntityBase>()};
            var endpointEntityInformation =
                metadata.Entities.Single(a => _apiClient.GetTypeForEntity(a.Name) == typeof (T));

            MainOperationMessage = string.Format(Translations.ContextManager_Update_Downloading_arg0, typeof (T).Name);

            if (!LastServerQueryDateTimeUtc.HasValue ||
                endpointEntityInformation.LastChangeDateTimeUtc > LastServerQueryDateTimeUtc)
            {
                result.Entities.AddRange(await _apiClient.GetEntitiesAsync<T>(LastServerQueryDateTimeUtc));
            }

            result.TruncateBeforeProcessing = LastServerQueryDateTimeUtc.HasValue &&
                                              LastServerQueryDateTimeUtc.Value <
                                              endpointEntityInformation.DeltaStartDateTimeUtc;

            MainOperationCurrentValue++;
            return result;
        }
    }
}