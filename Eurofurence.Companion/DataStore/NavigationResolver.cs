using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.DataModel;
using System.Linq;
using Eurofurence.Companion.DataStore.Abstractions;

namespace Eurofurence.Companion.DataStore
{
    [IocBeacon(TargetType = typeof(INavigationResolver), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class NavigationResolver : INavigationResolver
    {
        public void Resolve(IDataContext dataContext)
        {
            foreach (var e in dataContext.EventConferenceDays) e.Entries.Clear();
            foreach (var e in dataContext.EventConferenceRooms) e.Entries.Clear();
            foreach (var e in dataContext.EventConferenceTracks) e.Entries.Clear();

            foreach (var e in dataContext.EventEntries)
            {
                e.ConferenceDay = dataContext.EventConferenceDays.SingleOrDefault(a => a.Id == e.ConferenceDayId);
                e.ConferenceRoom = dataContext.EventConferenceRooms.SingleOrDefault(a => a.Id == e.ConferenceRoomId);
                e.ConferenceTrack = dataContext.EventConferenceTracks.SingleOrDefault(a => a.Id == e.ConferenceTrackId);
                e.Image = dataContext.Images.SingleOrDefault(a => a.Id == e.ImageId);
                

                e.ConferenceDay?.Entries.Add(e);
                e.ConferenceRoom?.Entries.Add(e);
                e.ConferenceTrack?.Entries.Add(e);

                e.AttributesProxy = new ExtensionProxy<DataModel.Api.EventEntry, DataModel.Local.EventEntryAttributes>(dataContext.EventEntryAttributes, e);
            }

            foreach (var e in dataContext.InfoGroups) e.Entries.Clear();
            foreach (var e in dataContext.Infos)
            {
                e.Group = dataContext.InfoGroups.SingleOrDefault(a => a.Id == e.InfoGroupId);

                e.Group?.Entries.Add(e);
            }

            foreach (var e in dataContext.Dealers)
            {
                e.ArtistImage = dataContext.Images.SingleOrDefault(a => a.Id == e.ArtistImageId);
                e.ArtistThumbnailImage = dataContext.Images.SingleOrDefault(a => a.Id == e.ArtistThumbnailImageId);
                e.ArtPreviewImage = dataContext.Images.SingleOrDefault(a => a.Id == e.ArtPreviewImageId);
            }
        }
    }
}
