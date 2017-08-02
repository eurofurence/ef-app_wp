using System;
using System.Collections.Generic;
using Eurofurence.Companion.DataModel.Api;
using System.Linq;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class EventConferenceRoomViewModel : BindableBase
    {
        private readonly Func<ICollection<EventEntryViewModel>> _eventEntryViewModelSelector;
        public EventConferenceRoom Entity { get; }

        public MapEntryViewModel MapEntry { get; set; }
        public bool HasMapEntry => MapEntry != null;

        public ICollection<EventEntryViewModel> EventEntries => _eventEntryViewModelSelector();

        public int EventEntryCount => EventEntries?.Count ?? 0;
        public double EventTotalHourCount => EventEntries?.Sum(a => a.Entity.Duration.TotalHours) ?? 0d;

        public EventConferenceRoomViewModel(
            EventConferenceRoom entity,
            Func<ICollection<EventEntryViewModel>> eventEntryViewModelSelector,
            IEnumerable<Map> maps = null
            )
        {
            Entity = entity;
            _eventEntryViewModelSelector = eventEntryViewModelSelector;

            var mapEntry = maps?.SelectMany(a => a.Entries).FirstOrDefault(
                a => a.Links.Any(link => link.FragmentType == LinkFragment.FragmentTypeEnum.EventConferenceRoom && link.Target == Entity.Id.ToString()));

            if (mapEntry != null)
            {
                MapEntry = new MapEntryViewModel(mapEntry) { Map = new MapViewModel(mapEntry.Map) };
                MapEntry.Map.Entries.Add(MapEntry);
            }
        }
    }
}