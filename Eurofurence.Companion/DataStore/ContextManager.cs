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
using System.Threading;

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
            ApplicationSettingsContext applicationSettingsContext
            )
        {
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
            public DataContextDataAreaEnum Area { get; set; }
        }

        public async Task InitializeAsync()
        {
            await _dataContext.LoadFromStoreAsync();
            _dataContext.RaiseRefreshed(DataContextDataAreaEnum.All);
        }

        public async Task ClearAll()
        {
            await _dataStore.ClearAllAsync();
            await _dataStore.ClearAllBlobsAsync();
            await _dataContext.LoadFromStoreAsync();

            _applicationSettingsContext.LastServerQueryDateTimeUtc = null;
            OnPropertyChanged(nameof(LastServerQueryDateTimeUtc));

            _dataContext.RaiseRefreshed(DataContextDataAreaEnum.All);
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
                var affectedAreas = DataContextDataAreaEnum.None;

                var sync = await _apiClient.GetDeltaAsync(LastServerQueryDateTimeUtc);

                if (sync.ConventionIdentifier != Consts.CONVENTION_IDENTIFIER)
                {
                    throw new InvalidOperationException($"Expected CID={Consts.CONVENTION_IDENTIFIER}, Server sent CID={sync.ConventionIdentifier}");
                }

                updateResults.Add(ProcessDelta(sync.Events, DataContextDataAreaEnum.Events));
                updateResults.Add(ProcessDelta(sync.EventConferenceDays, DataContextDataAreaEnum.Events));
                updateResults.Add(ProcessDelta(sync.EventConferenceRooms, DataContextDataAreaEnum.Events));
                updateResults.Add(ProcessDelta(sync.EventConferenceTracks, DataContextDataAreaEnum.Events));
                updateResults.Add(ProcessDelta(sync.Dealers, DataContextDataAreaEnum.Dealers));
                updateResults.Add(ProcessDelta(sync.KnowledgeGroups, DataContextDataAreaEnum.Knowledge));
                updateResults.Add(ProcessDelta(sync.KnowledgeEntries, DataContextDataAreaEnum.Knowledge));
                updateResults.Add(ProcessDelta(sync.Images, DataContextDataAreaEnum.Images));
                updateResults.Add(ProcessDelta(sync.Announcements, DataContextDataAreaEnum.Announcements));
                updateResults.Add(ProcessDelta(sync.Maps, DataContextDataAreaEnum.Maps));

                MainOperationMessage = $"{Translations.ContextManager_Update_DownloadingImageContent}...";

                await UpdateImageDataAsync(updateResults.Single(a => a.EntityType == typeof(Image)).Entities.Cast<Image>());

                MainOperationMessage = $"{Translations.ContextManager_Update_Synchronizing}...";

                foreach(var entity in updateResults)
                {
                    if (entity.TruncateBeforeProcessing)
                    {
                        await _dataStore.ClearAsync(entity.EntityType);
                        affectedAreas |= entity.Area;
                    }
                    if (entity.Entities.Count > 0)
                    {
                        affectedAreas |= entity.Area;
                    }
                }

                await _dataStore.ApplyDeltaAsync(updateResults.SelectMany(a => a.Entities), (current, max, id) =>
                {
                    //SubOperationMaxValue = (ulong)max;
                    //SubOperationCurrentValue = (ulong)current;
                });


                _applicationSettingsContext.LastServerQueryDateTimeUtc = sync.CurrentDateTimeUtc;
                OnPropertyChanged(nameof(LastServerQueryDateTimeUtc));


                // Don't trigger a reload if we didn't touch anything.
                if (updateResults.Any(a => a.TruncateBeforeProcessing || a.Entities.Count > 0))
                {
                    await _dataContext.LoadFromStoreAsync();
                    await _dataContext.SaveToStoreAsync();
                }

                _dataContext.RaiseRefreshed(affectedAreas);

                UpdateStatus = TaskStatus.RanToCompletion;
                MainOperationMessage = $"{Translations.ContextManager_Update_Done}!";

                return;
            }
            catch (Exception ex) // This probably fails when no connectivity is present.
            {
                UpdateStatus = TaskStatus.Faulted;
            }
        }

        private EntityUpdateResult ProcessDelta<T>(DeltaResponse<T> response, DataContextDataAreaEnum area) where T: EntityBase, new()
        {
            var result = new EntityUpdateResult {
                EntityType = typeof(T),
                Entities = new List<EntityBase>(),
                TruncateBeforeProcessing = response.RemoveAllBeforeInsert,
                Area = area
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

        private SemaphoreSlim _imageLimiter = new SemaphoreSlim(4, 4);

        private async Task UpdateImageDataAsync(IEnumerable<Image> images)
        {
            var imageList = images.ToList();

            MainOperationMaxValue = (ulong) imageList.Count;
            MainOperationCurrentValue = 0;

            var tasks = imageList.Select(imageEntity => Task.Run(async () =>
            {
                if (imageEntity.IsDeleted == 1)
                {
                    await _dataStore.ClearBlobAsync(imageEntity.Id, "ImageData");
                }
                else {
                    try
                    {
                        await _imageLimiter.WaitAsync();
                        var content = await _apiClient.GetImageContentAsBufferAsync(imageEntity.Id);
                        var bytes = content.ToArray();
                        await _dataStore.SaveBlobAsync(imageEntity.Id, "ImageData", bytes);
                    }
                    finally
                    {
                        _imageLimiter.Release();
                    }

                }

                MainOperationCurrentValue++;
            })).ToList();

            foreach (var task in tasks)
            {
                await task;
            }
        }
    }
}