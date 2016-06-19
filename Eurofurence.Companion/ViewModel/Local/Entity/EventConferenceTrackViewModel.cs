using System;
using System.Collections.Generic;
using Eurofurence.Companion.DataModel.Api;
using System.Linq;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class EventConferenceTrackViewModel : BindableBase
    {
        private readonly Func<ICollection<EventEntryViewModel>> _eventEntryViewModelSelector;
        public EventConferenceTrack Entity { get; }

        public ICollection<EventEntryViewModel> EventEntries => _eventEntryViewModelSelector();

        public int EventEntryCount => EventEntries?.Count ?? 0;
        public double EventTotalHourCount => EventEntries?.Sum(a => a.Entity.Duration.TotalHours) ?? 0d;

        public EventConferenceTrackViewModel(
            EventConferenceTrack entity, 
            Func<ICollection<EventEntryViewModel>> eventEntryViewModelSelector)
        {
            Entity = entity;
            _eventEntryViewModelSelector = eventEntryViewModelSelector;
        }
    }
}