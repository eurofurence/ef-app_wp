using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;
// ReSharper disable ExplicitCallerInfoArgument

namespace Eurofurence.Companion.ViewModel
{

    [IocBeacon(TargetType = typeof (MainViewModel), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class MainViewModel : BindableBase
    {
        private readonly EventsViewModel _eventsViewModel;
        private readonly ITimeProvider _timeProvider;
        private DateTime _lastFullMinute = DateTime.MinValue;

        public MainViewModel(ITimeProvider timeProvider, EventsViewModel eventsViewModel)
        {
            InitializeDispatcherFromCurrentThread();
            _eventsViewModel = eventsViewModel;
            _eventsViewModel.Invalidated += (sender, args) => UpdateUpcomingEventsData();

            UpcomingEvents = new ObservableCollection<EventEntryViewModel>();

            _timeProvider = timeProvider;
            _timeProvider.PropertyChanged += _timeProvider_PropertyChanged;

            UpdateUpcomingEventsData();
        }

        public DateTime CurrentDateTimeLocal => _timeProvider.CurrentDateTimeUtc;

        public ObservableCollection<EventEntryViewModel> UpcomingEvents { get; }

        private void UpdateUpcomingEventsData()
        {
            var allUpcomingEvents = _eventsViewModel.EventEntries
                .Where(a => a.Entity.EventDateTimeUtc >= CurrentDateTimeLocal
                            && a.Entity.EventDateTimeUtc.Day == CurrentDateTimeLocal.Day)
                .OrderBy(a => a.Entity.EventDateTimeUtc)
                .ToList();

            var allUpcomingEventsDistinctStartingTimes =
                allUpcomingEvents.Select(a => a.Entity.EventDateTimeUtc).Distinct().ToList();

            var eventsToDisplay = new List<EventEntryViewModel>();

            // Starting in the next 30 minutes.
            foreach (
                var startTime in
                    allUpcomingEventsDistinctStartingTimes.Where(
                        time => (time - CurrentDateTimeLocal).TotalMinutes <= 30).ToList())
            {
                eventsToDisplay.AddRange(allUpcomingEvents.Where(a => a.Entity.EventDateTimeUtc == startTime));
                allUpcomingEventsDistinctStartingTimes.Remove(startTime);
            }

            // Add complete chunks while we still have room.
            while (eventsToDisplay.Count < 4 && allUpcomingEventsDistinctStartingTimes.Count > 0)
            {
                var startTime = allUpcomingEventsDistinctStartingTimes[0];
                eventsToDisplay.AddRange(allUpcomingEvents.Where(a => a.Entity.EventDateTimeUtc == startTime));
                allUpcomingEventsDistinctStartingTimes.Remove(startTime);
            }

            foreach (var @event in UpcomingEvents.Where(a => !eventsToDisplay.Contains(a)).ToList())
            {
                UpcomingEvents.Remove(@event);
            }

            foreach (var @event in eventsToDisplay.Where(a => !UpcomingEvents.Contains(a)).ToList())
            {
                UpcomingEvents.Add(@event);
            }
        }

        private void _timeProvider_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_timeProvider.CurrentDateTimeUtc):
                    OnPropertyChanged(e.PropertyName);
                    break;

                case nameof(_timeProvider.CurrentDateTimeMinuteUtc):
                    UpdateUpcomingEventsData();
                    break;
            }
        }
    }
}