﻿using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DependencyResolution;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Appointments;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataStore.Abstractions;

namespace Eurofurence.Companion.ViewModel
{
    [IocBeacon(Scope =IocBeacon.ScopeEnum.Singleton)]
    public class EventsViewModel : BindableBase
    {
        private readonly IDataContext _dataContext;
        private readonly ITimeProvider _timeProvider;

        public ObservableCollection<EventEntryViewModel> EventEntries { get; }
        public ObservableCollection<EventConferenceDayViewModel> EventConferenceDays { get; }
        public ObservableCollection<EventConferenceRoomViewModel> EventConferenceRooms { get; }
        public ObservableCollection<EventConferenceTrackViewModel> EventConferenceTracks { get; }

        public event EventHandler Invalidated;

        public ICommand AddEventToCalendarCommand { get; set; }

        public ObservableCollection<EventEntryViewModel> EventEntrySearchResults { get; set; }

        private string _searchText = "";
        public string SearchText {
            get { return _searchText; }
            set
            {
                SetProperty(ref _searchText, value);
                UpdateSearchResults();
            }
        }

        private void UpdateSearchResults()
        {
            EventEntrySearchResults.Clear();
            if (String.IsNullOrWhiteSpace(_searchText)) return;

            foreach (var result in EventEntries
                .Where(e => e.Entity.Title.ToLower().Contains(_searchText.ToLower()))
                .OrderBy(e => e.ConferenceDay.Entity.Date)
                .ThenBy(e => e.Entity.StartTime)
                )
            {
                EventEntrySearchResults.Add(result);
            }
        }

        public EventsViewModel(IDataContext dataContext, ITimeProvider timeProvider)
        {
            InitializeDispatcherFromCurrentThread();

            _dataContext = dataContext;
            _dataContext.Refreshed += (sender, args) => { MapToViewModels(); };

            _timeProvider = timeProvider;
            EventEntrySearchResults = new ObservableCollection<EventEntryViewModel>();

            EventEntries = new ObservableCollection<EventEntryViewModel>();
            EventConferenceDays = new ObservableCollection<EventConferenceDayViewModel>();
            EventConferenceRooms = new ObservableCollection<EventConferenceRoomViewModel>();
            EventConferenceTracks = new ObservableCollection<EventConferenceTrackViewModel>();

            MapToViewModels();

            if (DesignMode.DesignModeEnabled) SearchText = "Furry";

            AddEventToCalendarCommand = new RelayCommand(async e =>
            {
                var entry = (EventEntry)e;

                var store = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);

                var appointment = new Appointment()
                {
                    StartTime = entry.ConferenceDay.Date + entry.StartTime,
                    Duration = entry.Duration,
                    Location = entry.ConferenceRoom.Name,
                    Subject = entry.Title,
                    Details = entry.Description,
                    Reminder = TimeSpan.FromMinutes(30),
                    AllowNewTimeProposal = false,
                    IsOrganizedByUser = false,
                    Sensitivity = AppointmentSensitivity.Private
                };

                await store.ShowEditNewAppointmentAsync(appointment);
            });
        }

        private void _dataContext_Refreshed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MapToViewModels()
        {
            EventEntries.Clear();
            EventConferenceDays.Clear();
            EventConferenceTracks.Clear();
            EventConferenceRooms.Clear();

            foreach (var entity in _dataContext.EventEntries) EventEntries.Add(new EventEntryViewModel(entity, _timeProvider));
            foreach (var entity in _dataContext.EventConferenceDays) EventConferenceDays.Add(new EventConferenceDayViewModel(entity, _timeProvider));
            foreach (var entity in _dataContext.EventConferenceTracks) EventConferenceTracks.Add(new EventConferenceTrackViewModel(entity));
            foreach (var entity in _dataContext.EventConferenceRooms) EventConferenceRooms.Add(new EventConferenceRoomViewModel(entity));

            foreach (var e in EventEntries)
            {
                e.ConferenceDay = EventConferenceDays.SingleOrDefault(a => a.Entity.Id == e.Entity.ConferenceDayId);
                e.ConferenceRoom = EventConferenceRooms.SingleOrDefault(a => a.Entity.Id == e.Entity.ConferenceRoomId);
                e.ConferenceTrack = EventConferenceTracks.SingleOrDefault(a => a.Entity.Id == e.Entity.ConferenceTrackId);

                e.ConferenceDay?.EventEntries.Add(e);
                e.ConferenceRoom?.EventEntries.Add(e);
                e.ConferenceTrack?.EventEntries.Add(e);
            }

            Invalidated?.Invoke(this, null);
        }
    }
}
