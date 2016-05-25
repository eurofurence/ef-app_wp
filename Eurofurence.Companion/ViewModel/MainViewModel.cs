using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.ViewModel
{
    public class EventConferenceDayViewModel : BindableBase
    {
        private readonly EventConferenceDay _entity;
        private readonly ITimeProvider _timeProvider;
        public EventConferenceDay Entity => _entity;

        public ObservableCollection<EventEntry> EventEntryViewModels { get; set; }

        public EventConferenceDayViewModel(EventConferenceDay entity, ITimeProvider timeProvider)
        {
            _entity = entity;
            _timeProvider = timeProvider;

            EventEntryViewModels = new ObservableCollection<EventEntry>();
        }
    }

    public class EventEntryViewModelProxy : Control
    {
        public static readonly DependencyProperty TimeProviderProperty =
            DependencyProperty.Register(
            "TimeProvider",
            typeof(ITimeProvider),
            typeof(EventEntryViewModelProxy), null
            );

        public static readonly DependencyProperty EventEntryProperty =
            DependencyProperty.Register(
            "EventEntry",
            typeof(EventEntry),
            typeof(EventEntryViewModelProxy), null
            );

        public ITimeProvider TimeProvider
        {
            get { return (ITimeProvider)GetValue(TimeProviderProperty); }
            set
            {
                SetValue(TimeProviderProperty, (ITimeProvider)value);
            }
        }

        public EventEntry EventEntry
        {
            get { return (EventEntry)GetValue(EventEntryProperty); }
            set
            {
                SetValue(EventEntryProperty, (EventEntry)value);
            }
        }

        public EventEntryViewModelProxy()
        {
            this.WatchProperty(nameof(EventEntry), (sender, args) =>
            Invalidate());
            this.WatchProperty(nameof(TimeProvider), (sender, args) =>
            Invalidate());
        }

        private void Invalidate()
        {
            if (EventEntry != null && TimeProvider != null)
            {
                ViewModel = new EventEntryViewModel(EventEntry, TimeProvider);
            }
        }

        public EventEntryViewModel ViewModel
        {
            get;
            set;
        }
    }

    public class EventEntryViewModel : BindableBase
    {
        private readonly EventEntry _entity;
        private readonly ITimeProvider _timeProvider;
        public EventEntry Entity => _entity;

        public EventEntryViewModel(EventEntry entity, ITimeProvider timeProvider)
        {
            _entity = entity;
            _timeProvider = timeProvider;

            _entity.AttributesProxy.Extension.WatchProperty(
                nameof(_entity.AttributesProxy.Extension.IsFavorite),
                _ => Invalidate());

            _timeProvider.WatchProperty(
                nameof(_timeProvider.CurrentDateTimeMinuteLocal),
                _ => Invalidate());
        }

        private void Invalidate()
        {
            TimeToStart = _entity.StartTimeAndDay.Value - _timeProvider.CurrentDateTimeLocal;
            IsStartingSoon = _entity.AttributesProxy.Extension.IsFavorite && TimeToStart.TotalMinutes <= 30;
        }

        private TimeSpan _timeToStart = TimeSpan.Zero;
        public TimeSpan TimeToStart {  get {  return _timeToStart;} set { SetProperty(ref _timeToStart, value); } }

        private bool _isStartingSoon = false;
        public bool IsStartingSoon { get { return _isStartingSoon; } set { SetProperty(ref _isStartingSoon, value); } }
    }


    [IocBeacon(TargetType = typeof (MainViewModel), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class MainViewModel : BindableBase
    {
        private readonly IDataContext _dataContext;
        private readonly ITimeProvider _timeProvider;
        private DateTime _lastFullMinute = DateTime.MinValue;

        public MainViewModel(ITimeProvider timeProvider, IDataContext dataContext)
        {
            InitializeDispatcherFromCurrentThread();
            _dataContext = dataContext;

            UpcomingEvents = new ObservableCollection<EventEntryViewModel>();

            _timeProvider = timeProvider;
            _timeProvider.PropertyChanged += _timeProvider_PropertyChanged;

            UpdateUpcomingEventsData();
        }

        public DateTime CurrentDateTimeLocal => _timeProvider.CurrentDateTimeLocal;

        public ObservableCollection<EventEntryViewModel> UpcomingEvents { get; }

        private void UpdateUpcomingEventsData()
        {
            var allUpcomingEvents = _dataContext.EventEntries
                .Where(a => a.StartTimeAndDay.HasValue
                            && a.StartTimeAndDay.Value >= CurrentDateTimeLocal
                            && a.StartTimeAndDay.Value.Day == CurrentDateTimeLocal.Day)
                .OrderBy(a => a.StartTimeAndDay.Value)
                .ToList();

            var allUpcomingEventsDistinctStartingTimes =
                allUpcomingEvents.Select(a => a.StartTimeAndDay.Value).Distinct().ToList();

            var eventsToDisplay = new List<EventEntry>();

            // Starting in the next 30 minutes.
            foreach (
                var startTime in
                    allUpcomingEventsDistinctStartingTimes.Where(
                        time => (time - CurrentDateTimeLocal).TotalMinutes <= 30).ToList())
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

            foreach (var @event in UpcomingEvents.Where(a => !eventsToDisplay.Contains(a.Entity)).ToList())
            {
                UpcomingEvents.Remove(@event);
            }

            foreach (var @event in eventsToDisplay.Where(a => UpcomingEvents.All(b => b.Entity != a)).ToList())
            {
                UpcomingEvents.Add(new EventEntryViewModel(@event, _timeProvider));;
            }
        }

        private void _timeProvider_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(_timeProvider.CurrentDateTimeLocal):
                    OnPropertyChanged(e.PropertyName);
                    break;

                case nameof(_timeProvider.CurrentDateTimeMinuteLocal):
                    UpdateUpcomingEventsData();
                    break;
            }
        }
    }
}