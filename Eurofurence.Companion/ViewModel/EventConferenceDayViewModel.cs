using System.Collections.ObjectModel;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.Common;
using System;

namespace Eurofurence.Companion.ViewModel
{
    public class EventConferenceDayViewModel : BindableBase
    {
        private readonly ITimeProvider _timeProvider;
        public EventConferenceDay Entity { get; }

        private bool _isCurrentDay = false;
        public bool IsCurrentDay { get { return _isCurrentDay; } set { SetProperty(ref _isCurrentDay, value); } }

        public ObservableCollection<EventEntryViewModel> EventEntries { get; set; }
        

        public EventConferenceDayViewModel(EventConferenceDay entity, ITimeProvider timeProvider)
        {
            Entity = entity;
            _timeProvider = timeProvider;

            EventEntries = new ObservableCollection<EventEntryViewModel>();

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