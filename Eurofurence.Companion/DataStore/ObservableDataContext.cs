using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataModel.Local;
using Eurofurence.Companion.DataModel.Abstractions;
using Eurofurence.Companion.DataStore.Abstractions;

namespace Eurofurence.Companion.DataStore
{
    [IocBeacon(TargetType = typeof(IDataContext), Scope = IocBeacon.ScopeEnum.Singleton, Environment = IocBeacon.EnvironmentEnum.RunTimeOnly)]
    public class ObservableDataContext : BindableBase, IDataContext
    {
        private readonly IDataStore _dataStore;
        private readonly INavigationResolver _navigationResolver;

        public event EventHandler<DataContextDataAreaEnum> Refreshed;

        public ObservableDataContext(IDataStore dataStore, INavigationResolver navigationResolver)
        {
            _dataStore = dataStore;
            _navigationResolver = navigationResolver;

            Announcements = new ObservableCollection<Announcement>();
            EventEntries = new ObservableCollection<EventEntry>();
            EventConferenceDays = new ObservableCollection<EventConferenceDay>();
            EventConferenceRooms = new ObservableCollection<EventConferenceRoom>();
            EventConferenceTracks = new ObservableCollection<EventConferenceTrack>();
            KnowledgeEntries = new ObservableCollection<KnowledgeEntry>();
            KnowledgeGroups = new ObservableCollection<KnowledgeGroup>();
            Images = new ObservableCollection<Image>();
            Dealers = new ObservableCollection<Dealer>();
            Maps = new ObservableCollection<Map>();
            MapEntries = new ObservableCollection<MapEntry>();

            EventEntryAttributes = new ObservableCollection<EventEntryAttributes>();

            _navigationResolver.Resolve(this);
        }

        public ObservableCollection<Announcement> Announcements { get; }
        public ObservableCollection<EventEntry> EventEntries { get; }
        public ObservableCollection<EventConferenceDay> EventConferenceDays { get; }
        public ObservableCollection<EventConferenceRoom> EventConferenceRooms { get; }
        public ObservableCollection<EventConferenceTrack> EventConferenceTracks { get; }
        public ObservableCollection<KnowledgeEntry> KnowledgeEntries { get; }
        public ObservableCollection<KnowledgeGroup> KnowledgeGroups { get; }
        public ObservableCollection<Image> Images { get; }
        public ObservableCollection<Dealer> Dealers { get; }
        public ObservableCollection<Map> Maps { get; }
        public ObservableCollection<MapEntry> MapEntries { get; }

        public ObservableCollection<EventEntryAttributes> EventEntryAttributes { get; }


        public async Task LoadFromStoreAsync()
        {
            await LoadEntityFromStoreAsync(Announcements, nameof(Announcements));
            await LoadEntityFromStoreAsync(EventEntries, nameof(EventEntries));
            await LoadEntityFromStoreAsync(EventConferenceDays, nameof(EventConferenceDays));
            await LoadEntityFromStoreAsync(EventConferenceRooms, nameof(EventConferenceRooms));
            await LoadEntityFromStoreAsync(EventConferenceTracks, nameof(EventConferenceTracks));
            await LoadEntityFromStoreAsync(KnowledgeEntries, nameof(KnowledgeEntries));
            await LoadEntityFromStoreAsync(KnowledgeGroups, nameof(KnowledgeGroups));
            await LoadEntityFromStoreAsync(Images, nameof(Images));
            await LoadEntityFromStoreAsync(Dealers, nameof(Dealers));
            await LoadEntityFromStoreAsync(Maps, nameof(Maps));
            await LoadEntityFromStoreAsync(MapEntries, nameof(MapEntries));
            await LoadEntityFromStoreAsync(EventEntryAttributes, nameof(EventEntryAttributes));
            _navigationResolver.Resolve(this);
        }


        public async Task SaveToStoreAsync()
        {
            foreach(var entity in EventEntryAttributes)
            {
                entity.IsDeleted = entity.GetPersistence() ? (byte)0 : (byte)1;
            }

            await _dataStore.ApplyDeltaAsync(EventEntryAttributes);
        }


        private async Task LoadEntityFromStoreAsync<T>(ICollection<T> target, string propertyName) where T : EntityBase, new()
        {
            try
            {
                var entities = await _dataStore.GetAsync<T>();

                if (typeof (ISortOrderKeyProvider).GetTypeInfo().IsAssignableFrom(typeof (T).GetTypeInfo()))
                {
                    entities = entities.OrderBy(a => (a as ISortOrderKeyProvider).SortOrderKey).ToList();
                }

                target.Clear();
                foreach (var entity in entities)
                {
                    target.Add(entity);
                }
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                // This should only happen when namespaces change - and we'll clear data there anyway.
            }
        }

        public void RaiseRefreshed(DataContextDataAreaEnum area)
        {
            Refreshed?.Invoke(this, area);
        }
    }
}