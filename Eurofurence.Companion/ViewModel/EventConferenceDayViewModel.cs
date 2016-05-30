using System.Collections.ObjectModel;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel
{
    public class EventConferenceDayViewModel : BindableBase
    {
        private readonly ITimeProvider _timeProvider;
        public EventConferenceDay Entity { get; }

        public ObservableCollection<EventEntryViewModel> EventEntries { get; set; }

        public EventConferenceDayViewModel(EventConferenceDay entity, ITimeProvider timeProvider)
        {
            Entity = entity;
            _timeProvider = timeProvider;

            EventEntries = new ObservableCollection<EventEntryViewModel>();
        }
    }
}