using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataModel.Local;

namespace Eurofurence.Companion.DataStore.Abstractions
{
    [Flags]
    public enum DataContextDataAreaEnum : int
    {
        All = int.MaxValue,
        None = 0,
        Announcements = 1 << 0,
        Events = 1 << 1,
        Knowledge = 1 << 2,
        Images = 1 << 3,
        Dealers = 1 << 4,
        Maps = 1 << 5
    }

    public interface IDataContext
    {
        event EventHandler<DataContextDataAreaEnum> Refreshed;

        void RaiseRefreshed(DataContextDataAreaEnum area);

        ObservableCollection<Announcement> Announcements { get; }
        ObservableCollection<EventEntry> EventEntries { get; }
        ObservableCollection<EventConferenceDay> EventConferenceDays { get; }
        ObservableCollection<EventConferenceRoom> EventConferenceRooms { get; }
        ObservableCollection<EventConferenceTrack> EventConferenceTracks { get; }
        ObservableCollection<KnowledgeEntry> KnowledgeEntries { get; }
        ObservableCollection<KnowledgeGroup> KnowledgeGroups { get; }
        ObservableCollection<Image> Images { get; }
        ObservableCollection<Dealer> Dealers { get; }
        ObservableCollection<Map> Maps { get; }

        ObservableCollection<EventEntryAttributes> EventEntryAttributes { get; }

        Task LoadFromStoreAsync();
        Task SaveToStoreAsync();
    }
}