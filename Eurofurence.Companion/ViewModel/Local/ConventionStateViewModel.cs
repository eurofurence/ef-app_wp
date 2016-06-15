using System.Linq;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel.Abstractions;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon(TargetType = typeof(ConventionStateViewModel), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class ConventionStateViewModel : BindableBase
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IEventsViewModelContext _eventsViewModelContext;

        public enum ConventionStateEnum
        {
            Ahead,
            Ongoing,
            Over
        }

        private string _conventionStateText;
        public string ConventionStateText
        {
            get { return _conventionStateText; }
            set
            {
                SetProperty(ref _conventionStateText, value);
            }
        }


        private ConventionStateEnum _conventionState = ConventionStateEnum.Ahead;
        public ConventionStateEnum ConventionState
        {
            get { return _conventionState; }
            set
            {
                SetProperty(ref _conventionState, value);
            }
        }

        public ConventionStateViewModel(ITimeProvider timeProvider, IEventsViewModelContext eventsViewModelContext)
        {
            InitializeDispatcherFromCurrentThread();

            _timeProvider = timeProvider;
            _timeProvider.WatchProperty(nameof(_timeProvider.CurrentDateTimeMinuteUtc), args => UpdateConventionState());

            _eventsViewModelContext = eventsViewModelContext;
            UpdateConventionState();
        }

        private void UpdateConventionState()
        {
            if (!_eventsViewModelContext.EventConferenceDays.Any()) return;

            var timeFrameStartUtc = _eventsViewModelContext.EventConferenceDays.Min(a => a.Entity.DayStartDateTimeUtc);
            var timeFrameEndUtc = _eventsViewModelContext.EventConferenceDays.Max(a => a.Entity.DayEndDateTimeUtc);

            if (_timeProvider.CurrentDateTimeUtc < timeFrameStartUtc)
            {
                ConventionState = ConventionStateEnum.Ahead;
            }
            else if (_timeProvider.CurrentDateTimeUtc > timeFrameEndUtc)
            {
                ConventionState = ConventionStateEnum.Over;
            }
            else
            {
                ConventionState = ConventionStateEnum.Ongoing;
            }
            ConventionStateText = ConventionState.ToString();
        }
    }
}