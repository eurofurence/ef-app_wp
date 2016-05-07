using Eurofurence.Companion.DataModel.Api;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using Eurofurence.Companion.ViewModel;

namespace Eurofurence.Companion.DataStore
{
    public class ObservableDataContext : BindableBase, IDataContext
    {
        public ObservableCollection<EventEntry> EventEntries { get; private set; }
        public ObservableCollection<EventConferenceDay> EventConferenceDays { get; private set; }
        public ObservableCollection<EventConferenceRoom> EventConferenceRooms { get; private set; }
        public ObservableCollection<EventConferenceTrack> EventConferenceTracks { get; private set; }
        public ObservableCollection<Info> Infos { get; private set; }
        public ObservableCollection<InfoGroup> InfoGroups { get; private set; }
        public ObservableCollection<Image> Images { get; private set; }

        private IDataStore _dataStore;
        private INavigationResolver _navigationResolver;

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

            _navigationResolver.Resolve(this);
        }

        public async Task RefreshAsync()
        {
            await LoadAsync(EventEntries, nameof(EventEntries));
            await LoadAsync(EventConferenceDays, nameof(EventConferenceDays));
            await LoadAsync(EventConferenceRooms, nameof(EventConferenceRooms));
            await LoadAsync(EventConferenceTracks, nameof(EventConferenceTracks));
            await LoadAsync(Infos, nameof(Infos));
            await LoadAsync(InfoGroups, nameof(InfoGroups));
            await LoadAsync(Images, nameof(Images));
            _navigationResolver.Resolve(this);
        }

        private Task BuildNavigation()
        {
            return Task.Delay(1);
        }

        private async Task LoadAsync<T>(ObservableCollection<T> target, string propertyName) where T : EntityBase, new()
        {
            var entities = await _dataStore.GetAsync<T>();

            if (typeof(ISortOrderKeyProvider).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
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
