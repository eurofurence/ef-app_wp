using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel
{
    public class EventConferenceRoomViewModel : BindableBase
    {
        public EventConferenceRoom Entity { get; }

        public ObservableCollection<EventEntryViewModel> EventEntries { get; set; }

        public EventConferenceRoomViewModel(EventConferenceRoom entity)
        {
            Entity = entity;

            EventEntries = new ObservableCollection<EventEntryViewModel>();
        }
    }
}