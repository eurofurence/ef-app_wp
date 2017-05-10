using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;


namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon(TargetType = typeof (UpcomingEventsViewModel), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class UpcomingEventsViewModel : BindableBase
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IEventsViewModelContext _eventsViewModelContext;

        private DateTime _lastFullMinute = DateTime.MinValue;

        public UpcomingEventsViewModel(ITimeProvider timeProvider, IEventsViewModelContext eventsViewModelContext)
        {
            InitializeDispatcherFromCurrentThread();

            _eventsViewModelContext = eventsViewModelContext;
            _eventsViewModelContext.Invalidated += (sender, args) => { Invalidate(); };

            _timeProvider = timeProvider;
            _timeProvider.WatchProperty(nameof(_timeProvider.CurrentDateTimeMinuteUtc), args => { Invalidate(); });
            
            UpcomingEvents = new ObservableCollection<EventEntryViewModel>();
            RunningEvents = new ObservableCollection<EventEntryViewModel>();

            Invalidate();
        }

        private void Invalidate()
        {
            UpdateRunningEventsData();
            UpdateUpcomingEventsData();
        }

        private void UpdateRunningEventsData()
        {
            var allRunningEvents = _eventsViewModelContext.EventEntries
                .Where(a => a.Entity.StartDateTimeUtc <= CurrentDateTimeUtc &&
                            a.Entity.EndDateTimeUtc >= CurrentDateTimeUtc)
                .OrderBy(a => a.Entity.ConferenceDay.Date)
                .ThenBy(a => a.Entity.StartDateTimeUtc)
                .ToList();

            foreach (var @event in RunningEvents.Where(a => !allRunningEvents.Contains(a)).ToList())
            {
                RunningEvents.Remove(@event);
            }

            foreach (var @event in allRunningEvents.Where(a => !RunningEvents.Contains(a)).ToList())
            {
                RunningEvents.Insert(0, @event);
            }
        }

        private DateTime CurrentDateTimeUtc => _timeProvider.CurrentDateTimeUtc;

        private EventConferenceDayViewModel _upcomingEventsConferenceDay;
        public EventConferenceDayViewModel UpcomingEventsConferenceDay { get { return _upcomingEventsConferenceDay; } set { SetProperty(ref _upcomingEventsConferenceDay, value); } }

        public ObservableCollection<EventEntryViewModel> UpcomingEvents { get; }
        public ObservableCollection<EventEntryViewModel> RunningEvents { get; }

        private void UpdateUpcomingEventsData()
        {
            var allUpcomingEvents = _eventsViewModelContext.EventEntries
                .Where(a => a.Entity.StartDateTimeUtc >= CurrentDateTimeUtc) // && a.Entity.EventDateTimeUtc.Day == CurrentDateTimeUtc.Day
                .OrderBy(a => a.Entity.ConferenceDay.Date)
                .ToList();

            var allUpcomingEventsDistinctStartingTimes =
                allUpcomingEvents.Select(a => a.Entity.StartDateTimeUtc).Distinct().ToList();

            var eventsToDisplay = new List<EventEntryViewModel>();

            // Starting in the next 30 minutes.
            foreach (
                var startTime in
                    allUpcomingEventsDistinctStartingTimes.Where(
                        time => (time - CurrentDateTimeUtc).TotalMinutes <= 30).ToList())
            {
                eventsToDisplay.AddRange(allUpcomingEvents.Where(a => a.Entity.StartDateTimeUtc == startTime));
                allUpcomingEventsDistinctStartingTimes.Remove(startTime);
            }

            // Add complete chunks while we still have room.
            while (eventsToDisplay.Count < 4 && allUpcomingEventsDistinctStartingTimes.Count > 0)
            {
                var startTime = allUpcomingEventsDistinctStartingTimes[0];
                eventsToDisplay.AddRange(allUpcomingEvents.Where(a => a.Entity.StartDateTimeUtc == startTime));
                allUpcomingEventsDistinctStartingTimes.Remove(startTime);
            }

            UpcomingEventsConferenceDay = eventsToDisplay.FirstOrDefault()?.ConferenceDay;

            foreach (var @event in UpcomingEvents.Where(a => !eventsToDisplay.Contains(a)).ToList())
            {
                UpcomingEvents.Remove(@event);
            }

            foreach (var @event in eventsToDisplay.Where(a => !UpcomingEvents.Contains(a)).ToList())
            {
                UpcomingEvents.Add(@event);
            }
        }
    }
}