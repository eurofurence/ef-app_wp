using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Eurofurence.Companion.ViewModel
{

    [IocBeacon(TargetType = typeof(MainViewModel), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class MainViewModel :BindableBase
    {
        private TimeProvider _timeProvider;
        private IDataContext _dataContext;

        public DateTime CurrentDateTimeLocal => _timeProvider.CurrentDateTimeLocal;

        public ObservableCollection<EventEntry> UpcomingEvents { get; private set; }


        public MainViewModel(TimeProvider timeProvider, IDataContext dataContext)
        {
            InitializeDispatcherFromCurrentThread();
            _dataContext = dataContext;

            UpcomingEvents = new ObservableCollection<EventEntry>();

            _timeProvider = timeProvider;
            _timeProvider.PropertyChanged += _timeProvider_PropertyChanged;

            UpdateData();
        }

        private void UpdateData()
        {
            var allUpcomingEvents = _dataContext.EventEntries
                .Where(a => a.StartTimeAndDay.HasValue 
                    && a.StartTimeAndDay.Value >= CurrentDateTimeLocal
                    && a.StartTimeAndDay.Value.Day == CurrentDateTimeLocal.Day)
                .OrderBy(a => a.StartTimeAndDay.Value)
                .ToList();

            var allUpcomingEventsDistinctStartingTimes = allUpcomingEvents.Select(a => a.StartTimeAndDay.Value).Distinct().ToList();

            var eventsToDisplay = new List<EventEntry>();

            // Starting in the next 30 minutes.
            foreach (var startTime in allUpcomingEventsDistinctStartingTimes.Where(time => (time - CurrentDateTimeLocal).TotalMinutes <= 30).ToList())
            {
                eventsToDisplay.AddRange(allUpcomingEvents.Where(a => a.StartTimeAndDay.Value == startTime));
                allUpcomingEventsDistinctStartingTimes.Remove(startTime);
            }

            // Add complete chunks while we still have room.
            while (eventsToDisplay.Count < 4 && allUpcomingEventsDistinctStartingTimes.Count > 0)
            {
                var startTime = allUpcomingEventsDistinctStartingTimes[0];
                eventsToDisplay.AddRange(allUpcomingEvents.Where(a => a.StartTimeAndDay.Value == startTime));
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

        private void _timeProvider_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentDateTimeLocal):
                    OnPropertyChanged(e.PropertyName);
                    UpdateData();
                    break;
            }
        }
    }
}
