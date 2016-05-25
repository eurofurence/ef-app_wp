using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Appointments;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace Eurofurence.Companion.ViewModel
{
    [IocBeacon]
    public class EventsViewModel : BindableBase
    {
        private readonly IDataContext _dataContext;
        private readonly ITimeProvider _timeProvider;

        public ObservableCollection<EventEntry> EventEntries => _dataContext.EventEntries;
        public ObservableCollection<EventConferenceDay> EventConferenceDays => _dataContext.EventConferenceDays;
        public ObservableCollection<EventConferenceRoom> EventConferenceRooms => _dataContext.EventConferenceRooms;
        public ObservableCollection<EventConferenceTrack> EventConferenceTracks => _dataContext.EventConferenceTracks;

        public ICommand AddEventToCalendarCommand { get; set; }

        public ObservableCollection<EventEntry> EventEntrySearchResults { get; set; }

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

            foreach(var result in EventEntries
                .Where(e => e.Title.ToLower().Contains(_searchText.ToLower()))
                .OrderBy(e => e.StartTime)
                .OrderBy(e => e.ConferenceDay.Date))
            {
                EventEntrySearchResults.Add(result);
            }
        }

        public EventsViewModel(IDataContext dataContext, ITimeProvider timeProvider)
        {
            _dataContext = dataContext;
            _timeProvider = timeProvider;
            EventEntrySearchResults = new ObservableCollection<EventEntry>();

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

    }
}
