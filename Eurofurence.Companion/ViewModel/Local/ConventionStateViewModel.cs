using System;
using System.IO;
using System.Linq;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon(TargetType = typeof(ConventionStateViewModel), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class ConventionStateViewModel : BindableBase
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IEventsViewModelContext _eventsViewModelContext;

        public enum ConventionStateEnum
        {
            Ahead,
            Ongoing,
            Over
        }


        private int _daysUntilFirstConventionDay = 0;
        public int DaysUntilFirstConventionDay
        {
            get { return _daysUntilFirstConventionDay; }
            set { SetProperty(ref _daysUntilFirstConventionDay, value); }
        }

        private bool _isAhead = false;
        private bool _isOngoing = false;
        private bool _isOver = false;

        public bool IsAhead { get { return _isAhead; } set { SetProperty(ref _isAhead, value); } }
        public bool IsOngoing { get { return _isOngoing; } set { SetProperty(ref _isOngoing, value); } }
        public bool IsOver { get { return _isOver; } set { SetProperty(ref _isOver, value); } }


        private ConventionStateEnum _conventionState;
        public ConventionStateEnum ConventionState {
            get { return _conventionState; }
            set { SetProperty(ref _conventionState, value); }
        }


        public ConventionStateViewModel(ITimeProvider timeProvider, IEventsViewModelContext eventsViewModelContext)
        {
            InitializeDispatcherFromCurrentThread();

            _timeProvider = timeProvider;
            _timeProvider.WatchProperty(nameof(_timeProvider.CurrentDateTimeMinuteUtc), args => Invalidate());

            _eventsViewModelContext = eventsViewModelContext;
            _eventsViewModelContext.Invalidated += (sender, args) => Invalidate();

            Invalidate();
        }

        private void Invalidate()
        {
            UpdateConventionState();
            UpdateDaysUntilFirstConventionDay();
        }

        private void UpdateDaysUntilFirstConventionDay()
        {
            if (!_eventsViewModelContext.EventConferenceDays.Any()) return;
            if (_conventionState != ConventionStateEnum.Ahead) return;

            var firstConventionDayStartDateTimeUtc = 
                _eventsViewModelContext.EventConferenceDays.Min(a => a.Entity.DayStartDateTimeUtc);

            DaysUntilFirstConventionDay = 
                (int)Math.Ceiling((firstConventionDayStartDateTimeUtc - _timeProvider.CurrentDateTimeUtc).TotalDays);
        }

        private void UpdateConventionState()
        {
            if (!_eventsViewModelContext.EventConferenceDays.Any()) return;

            var timeFrameStartUtc = _eventsViewModelContext.EventConferenceDays.Min(a => a.Entity.DayStartDateTimeUtc);
            var timeFrameEndUtc = _eventsViewModelContext.EventConferenceDays.Max(a => a.Entity.DayEndDateTimeUtc);

            if (_timeProvider.CurrentDateTimeUtc < timeFrameStartUtc)
            {
                ConventionState = ConventionStateEnum.Ahead;
            }
            else if (_timeProvider.CurrentDateTimeUtc > timeFrameEndUtc)
            {
                ConventionState = ConventionStateEnum.Over;
            }
            else
            {
                ConventionState = ConventionStateEnum.Ongoing;
            }

            IsAhead = ConventionState == ConventionStateEnum.Ahead;
            IsOngoing = ConventionState == ConventionStateEnum.Ongoing;
            IsOver = ConventionState == ConventionStateEnum.Over;
        }
    }
}