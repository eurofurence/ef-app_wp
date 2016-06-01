using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.ViewModel.Local;

namespace Eurofurence.Companion.ViewModel.Abstractions
{
    public interface IEventsViewModelContext
    {
        ObservableCollection<EventConferenceDayViewModel> EventConferenceDays { get; }
        ObservableCollection<EventConferenceRoomViewModel> EventConferenceRooms { get; }
        ObservableCollection<EventConferenceTrackViewModel> EventConferenceTracks { get; }
        ObservableCollection<EventEntryViewModel> EventEntries { get; }

        event EventHandler Invalidated;
    }
}