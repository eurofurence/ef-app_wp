using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel
{
    public class EventConferenceTrackViewModel : BindableBase
    {
        public EventConferenceTrack Entity { get; }

        public ObservableCollection<EventEntryViewModel> EventEntries { get; set; }

        public EventConferenceTrackViewModel(EventConferenceTrack entity)
        {
            Entity = entity;

            EventEntries = new ObservableCollection<EventEntryViewModel>();
        }
    }
}