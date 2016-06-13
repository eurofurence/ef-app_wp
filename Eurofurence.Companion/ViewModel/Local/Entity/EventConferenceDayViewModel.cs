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

        private bool _isCurrentDay = false;
        public bool IsCurrentDay { get { return _isCurrentDay; } set { SetProperty(ref _isCurrentDay, value); } }

        public ICollection<EventEntryViewModel> EventEntries => _eventEntryViewModelSelector();
        

        public EventConferenceDayViewModel(
            EventConferenceDay entity, 
            ITimeProvider timeProvider,
            Func<ICollection<EventEntryViewModel>> eventEntryViewModelSelector)
        {
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