using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class EventEntryViewModel : BindableBase
    {
        private readonly Func<EventConferenceDayViewModel> _conferenceDayViewModelSelector;
        private readonly Func<EventConferenceTrackViewModel> _conferenceTrackViewModelSelector;
        private readonly Func<EventConferenceRoomViewModel> _conferenceRoomViewModelSelector;
        private readonly ITimeProvider _timeProvider;

        public EventEntry Entity { get; }

        public EventConferenceDayViewModel ConferenceDay => _conferenceDayViewModelSelector();
        public EventConferenceTrackViewModel ConferenceTrack => _conferenceTrackViewModelSelector();
        public EventConferenceRoomViewModel ConferenceRoom => _conferenceRoomViewModelSelector();

        public EventEntryViewModel(
            EventEntry entity, 
            ITimeProvider timeProvider, 
            Func<EventConferenceDayViewModel> conferenceDayViewModelSelector,
            Func<EventConferenceTrackViewModel> conferenceTrackViewModelSelector,
            Func<EventConferenceRoomViewModel> conferenceRoomViewModelSelector)
        {
            InitializeDispatcherFromCurrentThread();

            Entity = entity;
            _timeProvider = timeProvider;
            _conferenceDayViewModelSelector = conferenceDayViewModelSelector;
            _conferenceTrackViewModelSelector = conferenceTrackViewModelSelector;
            _conferenceRoomViewModelSelector = conferenceRoomViewModelSelector;

            Entity.AttributesProxy.Extension.WatchProperty(
                nameof(Entity.AttributesProxy.Extension.IsFavorite),
                _ => Invalidate());

            _timeProvider.WatchProperty(
                nameof(_timeProvider.CurrentDateTimeUtc),
                _ => Invalidate());

            Invalidate();
        }

        private void Invalidate()
        {
            TimeToStart = Entity.StartDateTimeUtc - _timeProvider.CurrentDateTimeUtc;
            IsStartingSoon = TimeToStart.TotalMinutes <= 30;
            IsFavoriteAndStartingSoon = IsStartingSoon && Entity.AttributesProxy.Extension.IsFavorite;
        }

        public bool HasSubTitle => !string.IsNullOrEmpty(Entity?.SubTitle);
        public bool HasPosterImage => Entity?.PosterImageId.HasValue ?? false;
        public bool HasBannerImage => Entity?.BannerImageId.HasValue ?? false;

        private TimeSpan _timeToStart = TimeSpan.Zero;
        public TimeSpan TimeToStart { get { return _timeToStart; } set { SetProperty(ref _timeToStart, value); } }

        private bool _isStartingSoon = false;
        public bool IsStartingSoon { get { return _isStartingSoon; } set { SetProperty(ref _isStartingSoon, value); } }

        private bool _isFavoriteAndStartingSoon = false;
        public bool IsFavoriteAndStartingSoon { get { return _isFavoriteAndStartingSoon; } set { SetProperty(ref _isFavoriteAndStartingSoon, value); } }

        public string FontAwesomeDecoratorIcon
        {
            get
            {
                var icons = new List<string>();

                if (Entity.Tags != null)
                {
                    if (Entity.Tags.Contains("kage")) icons.Add("\uf188\uf000");
                    if (Entity.Tags.Contains("main_stage")) icons.Add("\uf069");
                    if (Entity.Tags.Contains("sponsors_only")) icons.Add("\uf123");
                    if (Entity.Tags.Contains("supersponsors_only")) icons.Add("\uf005");

                    if (Entity.Tags.Contains("art_show")) icons.Add("\uf03e");
                    if (Entity.Tags.Contains("dealers_den")) icons.Add("\uf07a");
                    if (Entity.Tags.Contains("photoshoot")) icons.Add("\uf030");
                }

                return string.Join(" ", icons);
            }
        }
    }
}