using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.ViewModel;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.DataModel.Local;
using Eurofurence.Companion.DataModel;
using System;

namespace Eurofurence.Companion.DataStore
{
    [IocBeacon(TargetType = typeof(IDataContext), Scope = IocBeacon.ScopeEnum.Singleton, Environment = IocBeacon.EnvironmentEnum.RunTimeOnly)]
    public class ObservableDataContext : BindableBase, IDataContext
    {
        private readonly IDataStore _dataStore;
        private readonly INavigationResolver _navigationResolver;

        public ObservableDataContext(IDataStore dataStore, INavigationResolver navigationResolver)
        {
            _dataStore = dataStore;
            _navigationResolver = navigationResolver;

            EventEntries = new ObservableCollection<EventEntry>();
            EventConferenceDays = new ObservableCollection<EventConferenceDay>();
            EventConferenceRooms = new ObservableCollection<EventConferenceRoom>();
            EventConferenceTracks = new ObservableCollection<EventConferenceTrack>();
            Infos = new ObservableCollection<Info>();
            InfoGroups = new ObservableCollection<InfoGroup>();
            Images = new ObservableCollection<Image>();
            Dealers = new ObservableCollection<Dealer>();

            EventEntryAttributes = new ObservableCollection<EventEntryAttributes>();

            _navigationResolver.Resolve(this);
        }

        public ObservableCollection<EventEntry> EventEntries { get; }
        public ObservableCollection<EventConferenceDay> EventConferenceDays { get; }
        public ObservableCollection<EventConferenceRoom> EventConferenceRooms { get; }
        public ObservableCollection<EventConferenceTrack> EventConferenceTracks { get; }
        public ObservableCollection<Info> Infos { get; }
        public ObservableCollection<InfoGroup> InfoGroups { get; }
        public ObservableCollection<Image> Images { get; }
        public ObservableCollection<Dealer> Dealers { get; }

        public ObservableCollection<EventEntryAttributes> EventEntryAttributes { get; }


        public async Task RefreshAsync()
        {
            await LoadAsync(EventEntries, nameof(EventEntries));
            await LoadAsync(EventConferenceDays, nameof(EventConferenceDays));
            await LoadAsync(EventConferenceRooms, nameof(EventConferenceRooms));
            await LoadAsync(EventConferenceTracks, nameof(EventConferenceTracks));
            await LoadAsync(Infos, nameof(Infos));
            await LoadAsync(InfoGroups, nameof(InfoGroups));
            await LoadAsync(Images, nameof(Images));
            await LoadAsync(Dealers, nameof(Dealers));
            await LoadAsync(EventEntryAttributes, nameof(EventEntryAttributes));
            _navigationResolver.Resolve(this);
        }


        public async Task SaveAsync()
        {
            foreach(var entity in EventEntryAttributes)
            {
                entity.IsDeleted = entity.IsPersistent ? (byte)0 : (byte)1;
            }

            await _dataStore.ApplyDeltaAsync(EventEntryAttributes)
                .ConfigureAwait(false);
        }


        private async Task LoadAsync<T>(ICollection<T> target, string propertyName) where T : EntityBase, new()
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


    }
}