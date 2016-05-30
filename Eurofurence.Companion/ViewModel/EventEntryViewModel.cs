using System;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel
{
    public class EventEntryViewModel : BindableBase
    {
        private readonly ITimeProvider _timeProvider;
        public EventEntry Entity { get; }

        public EventConferenceDayViewModel ConferenceDay { get; set; }
        public EventConferenceTrackViewModel ConferenceTrack { get; set; }
        public EventConferenceRoomViewModel ConferenceRoom { get; set; }

        public EventEntryViewModel(EventEntry entity, ITimeProvider timeProvider)
        {
            InitializeDispatcherFromCurrentThread();

            Entity = entity;
            _timeProvider = timeProvider;

            Entity.AttributesProxy.Extension.WatchProperty(
                nameof(Entity.AttributesProxy.Extension.IsFavorite),
                _ => Invalidate());

            _timeProvider.WatchProperty(
                nameof(_timeProvider.CurrentDateTimeUtc),
                _ => Invalidate());

            Invalidate();
        }

        private void Invalidate()
        {
            TimeToStart = Entity.EventDateTimeUtc - _timeProvider.CurrentDateTimeUtc;
            IsStartingSoon = Entity.AttributesProxy.Extension.IsFavorite && TimeToStart.TotalMinutes <= 30;
        }

        private TimeSpan _timeToStart = TimeSpan.Zero;
        public TimeSpan TimeToStart { get { return _timeToStart; } set { SetProperty(ref _timeToStart, value); } }

        private bool _isStartingSoon = false;
        public bool IsStartingSoon { get { return _isStartingSoon; } set { SetProperty(ref _isStartingSoon, value); } }
    }
}