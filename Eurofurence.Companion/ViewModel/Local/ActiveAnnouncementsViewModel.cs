using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataStore.Abstractions;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Eurofurence.Companion.DataModel.Api;
using System;
using System.Linq;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon]
    public class ActiveAnnouncementsViewModel : BindableBase
    {
        private IDataContext _dataContext;
        private ITimeProvider _timeProvider;

        private bool _hasActiveAnnouncements = false;
        public bool HasActiveAnnouncements { get { return _hasActiveAnnouncements; } set { SetProperty(ref _hasActiveAnnouncements, value); } }

        public ObservableCollection<Announcement> ActiveAnnouncements { get; set; }

        public ActiveAnnouncementsViewModel(IDataContext dataContext, ITimeProvider timeProvider)
        {
            InitializeDispatcherFromCurrentThread();

            _dataContext = dataContext;
            _dataContext.Refreshed += (o, e) => { UpdateAnnouncements(); };
            _timeProvider = timeProvider;

            _timeProvider.PropertyChanged += _timeProvider_PropertyChanged;

            ActiveAnnouncements = new ObservableCollection<Announcement>();
            UpdateAnnouncements();
        }

        private void _timeProvider_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_timeProvider.CurrentDateTimeMinuteUtc))
                UpdateAnnouncements();
        }

        private void UpdateAnnouncements()
        {
            var activeAnnouncements = _dataContext.Announcements.Where(
                a => a.ValidFromDateTimeUtc <= _timeProvider.CurrentDateTimeUtc &&
                a.ValidUntilDateTimeUtc >= _timeProvider.CurrentDateTimeUtc)
                .ToList();

            foreach(var toRemove in ActiveAnnouncements.Where(a => !activeAnnouncements.Contains(a)).ToList())
            {
                ActiveAnnouncements.Remove(toRemove);
            }

            foreach(var toAdd in activeAnnouncements.Where(a => !ActiveAnnouncements.Contains(a)).ToList())
            {
                ActiveAnnouncements.Add(toAdd);
            }

            HasActiveAnnouncements = ActiveAnnouncements.Count > 0;
        }
    }
}
