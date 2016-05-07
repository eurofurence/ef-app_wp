using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.DataStore
{
    public interface IDataContext
    {
        ObservableCollection<EventEntry> EventEntries { get; }
        ObservableCollection<EventConferenceDay> EventConferenceDays { get; }
        ObservableCollection<EventConferenceRoom> EventConferenceRooms { get; }
        ObservableCollection<EventConferenceTrack> EventConferenceTracks { get; }
        ObservableCollection<Info> Infos { get; }
        ObservableCollection<InfoGroup> InfoGroups { get; }
        ObservableCollection<Image> Images { get; }

        Task RefreshAsync();
    }
}