using System;
using System.Collections.Generic;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class EventConferenceDayViewModel : BindableBase
    {
        private readonly ITimeProvider _timeProvider;
        private readonly Func<ICollection<EventEntryViewModel>> _eventEntryViewModelSelector;
        public EventConferenceDay Entity { get; }

        public EventConferenceDayViewModel PreviousDay { get; set; }
        public bool HasPreviousDay => PreviousDay != null;
        public EventConferenceDayViewModel NextDay { get; set; }
        public bool HasNextDay => NextDay != null;

        private bool _isSelected = false;
        public bool IsSelected { get { return _isSelected; } set { SetProperty(ref _isSelected, value); } }

        private bool _isCurrentDay = false;
        public bool IsCurrentDay { get { return _isCurrentDay; } set { SetProperty(ref _isCurrentDay, value); } }

        public ICollection<EventEntryViewModel> EventEntries => _eventEntryViewModelSelector();
        

        public EventConferenceDayViewModel(
            EventConferenceDay entity, 
            ITimeProvider timeProvider,
            Func<ICollection<EventEntryViewModel>> eventEntryViewModelSelector)
        {
            InitializeDispatcherFromCurrentThread();

            Entity = entity;
            _eventEntryViewModelSelector = eventEntryViewModelSelector;

            _timeProvider = timeProvider;
            _timeProvider.WatchProperty(
                nameof(_timeProvider.CurrentDateTimeMinuteUtc),
                _ => Invalidate());

            Invalidate();
        }

        private void Invalidate()
        {
            IsCurrentDay =
                _timeProvider.CurrentDateTimeMinuteUtc >= Entity.DayStartDateTimeUtc
                && _timeProvider.CurrentDateTimeMinuteUtc < Entity.DayEndDateTimeUtc;
        }
    }
}