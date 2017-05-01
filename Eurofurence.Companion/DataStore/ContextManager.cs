using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataModel.Local;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Newtonsoft.Json;

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

            UpdateCommand = new AwaitableCommand(() => Update());
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
            await _dataContext.LoadFromStoreAsync();
        }

        public async Task ClearAll()
        {
            await _dataStore.ClearAllAsync();
            await _dataStore.ClearAllBlobsAsync();
            await _dataContext.LoadFromStoreAsync();

            _applicationSettingsContext.LastServerQueryDateTimeUtc = null;
            OnPropertyChanged(nameof(LastServerQueryDateTimeUtc));
        }


        public async Task Update(bool doSaveToStoreBeforeUpdate = true)
        {
            UpdateStatus = TaskStatus.Running;
            try
            {
                MainOperationMessage = $"{Translations.ContextManager_Update_Initializing}...";

                if (doSaveToStoreBeforeUpdate)
                {
                    await _dataContext.SaveToStoreAsync();
                }

                var updateResults = new List<EntityUpdateResult>();

                var sync = await _apiClient.GetDeltaAsync(LastServerQueryDateTimeUtc);

                updateResults.Add(ProcessDelta(sync.Events));
                updateResults.Add(ProcessDelta(sync.EventConferenceDays));
                updateResults.Add(ProcessDelta(sync.EventConferenceRooms));
                updateResults.Add(ProcessDelta(sync.EventConferenceTracks));
                updateResults.Add(ProcessDelta(sync.Dealers));
                updateResults.Add(ProcessDelta(sync.KnowledgeGroups));
                updateResults.Add(ProcessDelta(sync.KnowledgeEntries));
                updateResults.Add(ProcessDelta(sync.Images));

                await UpdateImageDataAsync(updateResults.Single(a => a.EntityType == typeof(Image)).Entities.Cast<Image>());


                foreach (var entity in updateResults.Where(a => a.TruncateBeforeProcessing).Select(b => b.EntityType))
                {
                    await _dataStore.ClearAsync(entity);
                }

                await _dataStore.ApplyDeltaAsync(updateResults.SelectMany(a => a.Entities), (current, max, id) =>
                {
                    SubOperationMaxValue = (ulong)max;
                    SubOperationCurrentValue = (ulong)current;
                });


                _applicationSettingsContext.LastServerQueryDateTimeUtc = sync.CurrentDateTimeUtc;
                OnPropertyChanged(nameof(LastServerQueryDateTimeUtc));

                // Don't trigger a reload if we didn't touch anything.
                if (updateResults.Any(a => a.TruncateBeforeProcessing || a.Entities.Count > 0))
                {
                    await _dataContext.LoadFromStoreAsync();
                    await _dataContext.SaveToStoreAsync();
                }


                UpdateStatus = TaskStatus.RanToCompletion;
                return;

                var metadata = await _apiClient.GetEndpointMetadataAsync();
                


                updateResults.Add(await UpdateEntitiesAsync<Announcement>(metadata));
                updateResults.Add(await UpdateEntitiesAsync<EventEntry>(metadata));
                updateResults.Add(await UpdateEntitiesAsync<EventConferenceDay>(metadata));
                updateResults.Add(await UpdateEntitiesAsync<EventConferenceRoom>(metadata));
                updateResults.Add(await UpdateEntitiesAsync<EventConferenceTrack>(metadata));
                updateResults.Add(await UpdateEntitiesAsync<Info>(metadata));
                updateResults.Add(await UpdateEntitiesAsync<InfoGroup>(metadata));
                updateResults.Add(await UpdateEntitiesAsync<Image>(metadata));
                updateResults.Add(await UpdateEntitiesAsync<Dealer>(metadata));
                updateResults.Add(await UpdateEntitiesAsync<Map>(metadata));
                updateResults.Add(await UpdateEntitiesAsync<MapEntry>(metadata));

                await UpdateImageDataAsync(updateResults.Single(a => a.EntityType == typeof(Image)).Entities.Cast<Image>());

                MainOperationMessage = Translations.ContextManager_Update_Initializing;

                foreach (var entity in updateResults.Where(a => a.TruncateBeforeProcessing).Select(b => b.EntityType))
                {
                    await _dataStore.ClearAsync(entity);
                }

                await _dataStore.ApplyDeltaAsync(updateResults.SelectMany(a => a.Entities), (current, max, id) =>
                {
                    SubOperationMaxValue = (ulong)max;
                    SubOperationCurrentValue = (ulong)current;
                });

                _applicationSettingsContext.LastServerQueryDateTimeUtc = metadata.CurrentDateTimeUtc;
                OnPropertyChanged(nameof(LastServerQueryDateTimeUtc));

                // Don't trigger a reload if we didn't touch anything.
                if (updateResults.Any(a => a.TruncateBeforeProcessing || a.Entities.Count > 0))
                {
                    await _dataContext.LoadFromStoreAsync();
                    await _dataContext.SaveToStoreAsync();
                }

                MainOperationMessage = $"{Translations.ContextManager_Update_Done}!";

                //if (Debugger.IsAttached)
                //{
                //    var storageInfo = await _dataStore.GetStorageFileSizesAsync();
                //    foreach (var row in storageInfo)
                //    {
                //        Debug.WriteLine($"{row.Key}: {row.Value} bytes");
                //    }
                //}

                UpdateStatus = TaskStatus.RanToCompletion;
            }
            catch (Exception) // This probably fails when no connectivity is present.
            {
                UpdateStatus = TaskStatus.Faulted;
            }

        }

        private EntityUpdateResult ProcessDelta<T>(DeltaResponse<T> response) where T: EntityBase, new()
        {
            var result = new EntityUpdateResult {
                EntityType = typeof(T),
                Entities = new List<EntityBase>(),
                TruncateBeforeProcessing = response.RemoveAllBeforeInsert
            };

            foreach(var id in response.DeletedEntities)
            {
                var x = new T();
                x.Id = id;
                x.IsDeleted = 1;

                result.Entities.Add(x);
            }

            foreach(var record in response.ChangedEntities)
            {
                result.Entities.Add(record);
            }

            return result;
        }

        private async Task UpdateImageDataAsync(IEnumerable<Image> images)
        {
            var imageList = images.ToList();

            MainOperationMessage = $"{Translations.ContextManager_Update_DownloadingImageContent}...";

            SubOperationMaxValue = (ulong) imageList.Count;
            SubOperationCurrentValue = 0;

            var tasks = imageList.Select(imageEntity => Task.Run(async () =>
            {
                if (imageEntity.IsDeleted == 1)
                {
                    await _dataStore.ClearBlobAsync(imageEntity.Id, "ImageData");
                }
                else {
                    var content = await _apiClient.GetImageContentAsBufferAsync(imageEntity.Id);
                    var bytes = content.ToArray();
                    await _dataStore.SaveBlobAsync(imageEntity.Id, "ImageData", bytes);
                }

                SubOperationCurrentValue++;
            })).ToList();

            foreach (var task in tasks)
            {
                await task;
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