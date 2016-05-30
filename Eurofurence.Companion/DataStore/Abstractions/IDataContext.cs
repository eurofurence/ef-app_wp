using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataModel.Local;

namespace Eurofurence.Companion.DataStore.Abstractions
{
    public interface IDataContext
    {
        event EventHandler Refreshed;

        ObservableCollection<EventEntry> EventEntries { get; }
        ObservableCollection<EventConferenceDay> EventConferenceDays { get; }
        ObservableCollection<EventConferenceRoom> EventConferenceRooms { get; }
        ObservableCollection<EventConferenceTrack> EventConferenceTracks { get; }
        ObservableCollection<Info> Infos { get; }
        ObservableCollection<InfoGroup> InfoGroups { get; }
        ObservableCollection<Image> Images { get; }
        ObservableCollection<Dealer> Dealers { get; }

        ObservableCollection<EventEntryAttributes> EventEntryAttributes { get; }

        Task RefreshAsync();
        Task SaveAsync();
    }
}