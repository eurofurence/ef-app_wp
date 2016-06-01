using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local;

namespace Eurofurence.Companion.ViewModel
{
    [IocBeacon(TargetType = typeof (IEventsViewModelContext), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class EventsViewModelContext : BindableBase, IEventsViewModelContext
    {
        private readonly IDataContext _dataContext;

        private readonly Dictionary<EventConferenceDay, EventConferenceDayViewModel> _eventConferenceDaysMapping
            = new Dictionary<EventConferenceDay, EventConferenceDayViewModel>();

        private readonly Dictionary<EventConferenceRoom, EventConferenceRoomViewModel> _eventConferenceRoomsMapping
            = new Dictionary<EventConferenceRoom, EventConferenceRoomViewModel>();

        private readonly Dictionary<EventConferenceTrack, EventConferenceTrackViewModel> _eventConferenceTracksMapping
            = new Dictionary<EventConferenceTrack, EventConferenceTrackViewModel>();

        private readonly Dictionary<EventEntry, EventEntryViewModel> _eventEntryMapping
            = new Dictionary<EventEntry, EventEntryViewModel>();

        private readonly ITimeProvider _timeProvider;

        public ObservableCollection<EventEntryViewModel> EventEntries { get; }
        public ObservableCollection<EventConferenceDayViewModel> EventConferenceDays { get; }
        public ObservableCollection<EventConferenceRoomViewModel> EventConferenceRooms { get; }
        public ObservableCollection<EventConferenceTrackViewModel> EventConferenceTracks { get; }


        public EventsViewModelContext(IDataContext dataContext, ITimeProvider timeProvider)
        {
            InitializeDispatcherFromCurrentThread();

            _dataContext = dataContext;
            _dataContext.Refreshed += (sender, args) => { MapToViewModels(); };

            _timeProvider = timeProvider;

            EventEntries = new ObservableCollection<EventEntryViewModel>();
            EventConferenceDays = new ObservableCollection<EventConferenceDayViewModel>();
            EventConferenceRooms = new ObservableCollection<EventConferenceRoomViewModel>();
            EventConferenceTracks = new ObservableCollection<EventConferenceTrackViewModel>();

            MapToViewModels();
        }

        public event EventHandler Invalidated;


        private void Populate<TEntity, TViewModel>(
            IEnumerable<TEntity> source, ICollection<TViewModel> store, IDictionary<TEntity, TViewModel> mapping,
            Func<TEntity, TViewModel> factory)
        {
            store.Clear();
            mapping.Clear();

            foreach (var entity in source)
            {
                var viewModel = factory(entity);
                mapping.Add(entity, viewModel);
                store.Add(viewModel);
            }
        }

        private void MapToViewModels()
        {
            Populate(_dataContext.EventEntries, EventEntries, _eventEntryMapping, entity =>
                new EventEntryViewModel(
                    entity,
                    _timeProvider,
                    () => _eventConferenceDaysMapping[entity.ConferenceDay],
                    () => _eventConferenceTracksMapping[entity.ConferenceTrack],
                    () => _eventConferenceRoomsMapping[entity.ConferenceRoom]
                    )
                );

            Populate(_dataContext.EventConferenceDays, EventConferenceDays, _eventConferenceDaysMapping, entity =>
                new EventConferenceDayViewModel(
                    entity,
                    _timeProvider,
                    () => entity.Entries.Select(e => _eventEntryMapping[e]).ToList()
                    )
                );

            Populate(_dataContext.EventConferenceRooms, EventConferenceRooms, _eventConferenceRoomsMapping, entity =>
                new EventConferenceRoomViewModel(entity,
                    () => entity.Entries.Select(e => _eventEntryMapping[e]).ToList()
                    )
                );

            Populate(_dataContext.EventConferenceTracks, EventConferenceTracks, _eventConferenceTracksMapping, entity =>
                new EventConferenceTrackViewModel(entity,
                    () => entity.Entries.Select(e => _eventEntryMapping[e]).ToList()
                    )
                );

            Invalidated?.Invoke(this, null);
        }
    }
}