using System;
using System.Collections.Generic;
using Eurofurence.Companion.DataModel.Api;
using System.Linq;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class EventConferenceRoomViewModel : BindableBase
    {
        private readonly Func<ICollection<EventEntryViewModel>> _eventEntryViewModelSelector;
        public EventConferenceRoom Entity { get; }

        public ICollection<EventEntryViewModel> EventEntries => _eventEntryViewModelSelector();

        public int EventEntryCount => EventEntries?.Count ?? 0;
        public double EventTotalHourCount => EventEntries?.Sum(a => a.Entity.Duration.TotalHours) ?? 0d;

        public EventConferenceRoomViewModel(
            EventConferenceRoom entity,
            Func<ICollection<EventEntryViewModel>> eventEntryViewModelSelector)
        {
            Entity = entity;
            _eventEntryViewModelSelector = eventEntryViewModelSelector;
        }
    }
}