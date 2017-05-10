using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Appointments;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon]
    public class EventsViewModel : BindableBase
    {
        private readonly IEventsViewModelContext _eventsViewModelContext;

        public ObservableCollection<EventEntryViewModel> EventEntries 
            => _eventsViewModelContext.EventEntries;

        public ObservableCollection<EventConferenceDayViewModel> EventConferenceDays
            => _eventsViewModelContext.EventConferenceDays;

        public ObservableCollection<EventConferenceRoomViewModel> EventConferenceRooms
            => _eventsViewModelContext.EventConferenceRooms;

        public ObservableCollection<EventConferenceTrackViewModel> EventConferenceTracks
            => _eventsViewModelContext.EventConferenceTracks;

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
            if (string.IsNullOrWhiteSpace(_searchText)) return;

            foreach (var result in EventEntries
                .Where(e => e.Entity.Title.ToLower().Contains(_searchText.ToLower()))
                .OrderBy(e => e.ConferenceDay.Entity.Date)
                .ThenBy(e => e.Entity.StartTime)
                )
            {
                EventEntrySearchResults.Add(result);
            }
        }


        public EventsViewModel(IEventsViewModelContext eventsViewModelContext)
        {
            InitializeDispatcherFromCurrentThread();
            _eventsViewModelContext = eventsViewModelContext;

            EventEntrySearchResults = new ObservableCollection<EventEntryViewModel>();

            if (DesignMode.DesignModeEnabled) SearchText = "Furry";

            AddEventToCalendarCommand = new RelayCommand(AddEventToCalendarAsync);
        }

        private async void AddEventToCalendarAsync(object parameter)
        {
            var entry = (EventEntryViewModel)parameter;

            var store = await AppointmentManager.RequestStoreAsync(AppointmentStoreAccessType.AllCalendarsReadOnly);

            var appointment = new Appointment()
            {
                StartTime =  entry.Entity.StartDateTimeUtc,
                Duration = entry.Entity.Duration,
                Location = entry.Entity.ConferenceRoom.Name,
                Subject = entry.Entity.Title,
                Details = entry.Entity.Description,
                Reminder = TimeSpan.FromMinutes(30),
                AllowNewTimeProposal = false,
                IsOrganizedByUser = false,
                Sensitivity = AppointmentSensitivity.Private
            };

            await store.ShowEditNewAppointmentAsync(appointment);
        }
    }
}
