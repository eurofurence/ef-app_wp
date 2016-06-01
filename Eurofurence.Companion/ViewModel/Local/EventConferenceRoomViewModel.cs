using System;
using System.Collections.Generic;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel.Local
{
    public class EventConferenceRoomViewModel : BindableBase
    {
        private readonly Func<ICollection<EventEntryViewModel>> _eventEntryViewModelSelector;
        public EventConferenceRoom Entity { get; }

        public ICollection<EventEntryViewModel> EventEntries => _eventEntryViewModelSelector();

        public EventConferenceRoomViewModel(
            EventConferenceRoom entity,
            Func<ICollection<EventEntryViewModel>> eventEntryViewModelSelector)
        {
            Entity = entity;
            _eventEntryViewModelSelector = eventEntryViewModelSelector;
        }
    }
}