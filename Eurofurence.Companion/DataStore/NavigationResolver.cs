﻿using Eurofurence.Companion.DependencyResolution;
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
                e.PosterImage = dataContext.Images.SingleOrDefault(a => a.Id == e.PosterImageId);
                e.BannerImage = dataContext.Images.SingleOrDefault(a => a.Id == e.BannerImageId);


                e.ConferenceDay?.Entries.Add(e);
                e.ConferenceRoom?.Entries.Add(e);
                e.ConferenceTrack?.Entries.Add(e);

                e.AttributesProxy = new ExtensionProxy<DataModel.Api.EventEntry, DataModel.Local.EventEntryAttributes>(dataContext.EventEntryAttributes, e);
            }

            foreach (var e in dataContext.KnowledgeGroups) e.Entries.Clear();
            foreach (var e in dataContext.KnowledgeEntries)
            {
                e.Group = dataContext.KnowledgeGroups.SingleOrDefault(a => a.Id == e.KnowledgeGroupId);

                e.Group?.Entries.Add(e);
            }

            foreach (var e in dataContext.Dealers)
            {
                e.ArtistImage = dataContext.Images.SingleOrDefault(a => a.Id == e.ArtistImageId);
                e.ArtistThumbnailImage = dataContext.Images.SingleOrDefault(a => a.Id == e.ArtistThumbnailImageId);
                e.ArtPreviewImage = dataContext.Images.SingleOrDefault(a => a.Id == e.ArtPreviewImageId);
            }

            foreach (var e in dataContext.KnowledgeEntries)
            {
                e.Images.Clear();
                if (e.ImageIds != null)
                {
                    e.Images.AddRange(e.ImageIds.Select(f => dataContext.Images.SingleOrDefault(a => a.Id == f)));
                }
            }

            foreach (var e in dataContext.Maps)
            {
                e.Image = dataContext.Images.SingleOrDefault(a => a.Id == e.ImageId);


                foreach(var me in e.Entries)
                {
                    me.Map = e;
                    foreach (var link in me.Links)
                    {
                        switch (link.FragmentType)
                        {
                            case DataModel.Api.LinkFragment.FragmentTypeEnum.DealerDetail:
                                me.TargetEntity = dataContext.Dealers.SingleOrDefault(a => a.Id.ToString() == link.Target);

                                break;

                        }
                    }
                }


            }          
        }
    }
}
