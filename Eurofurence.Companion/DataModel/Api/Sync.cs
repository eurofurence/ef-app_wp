﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eurofurence.Companion.DataModel.Api
{
    public class AggregatedDeltaResponse
    {
        public DateTime? Since { get; set; }
        public DateTime CurrentDateTimeUtc { get; set; }

        public DeltaResponse<EventEntry> Events { get; set; }
        public DeltaResponse<EventConferenceDay> EventConferenceDays { get; set; }
        public DeltaResponse<EventConferenceRoom> EventConferenceRooms { get; set; }
        public DeltaResponse<EventConferenceTrack> EventConferenceTracks { get; set; }
        public DeltaResponse<InfoGroup> KnowledgeGroups { get; set; }
        public DeltaResponse<Info> KnowledgeEntries { get; set; }
        public DeltaResponse<Image> Images { get; set; }
        public DeltaResponse<Dealer> Dealers { get; set; }
    }


    public class DeltaResponse<T> where T : EntityBase
    {
        public DateTime StorageLastChangeDateTimeUtc { get; set; }
        public DateTime StorageDeltaStartChangeDateTimeUtc { get; set; }

        public bool RemoveAllBeforeInsert { get; set; }

        public T[] ChangedEntities { get; set; }
        public Guid[] DeletedEntities { get; set; }
    }
}