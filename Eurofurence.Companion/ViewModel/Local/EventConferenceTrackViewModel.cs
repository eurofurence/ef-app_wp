using System;
using System.Collections.Generic;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel.Local
{
    public class EventConferenceTrackViewModel : BindableBase
    {
        private readonly Func<ICollection<EventEntryViewModel>> _eventEntryViewModelSelector;
        public EventConferenceTrack Entity { get; }

        public ICollection<EventEntryViewModel> EventEntries => _eventEntryViewModelSelector();

        public EventConferenceTrackViewModel(
            EventConferenceTrack entity, 
            Func<ICollection<EventEntryViewModel>> eventEntryViewModelSelector)
        {
            Entity = entity;
            _eventEntryViewModelSelector = eventEntryViewModelSelector;
        }
    }
}