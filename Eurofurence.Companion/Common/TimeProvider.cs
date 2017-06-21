using Eurofurence.Companion.DependencyResolution;
using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataModel;
// ReSharper disable ExplicitCallerInfoArgument

namespace Eurofurence.Companion.Common
{
    [IocBeacon(TargetType = typeof(ITimeProvider), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class TimeProvider : BindableBase, ITimeProvider
    {
        private DateTime _currentDateTimeUtc = DateTime.UtcNow;
        private DateTime _currentDateTimeMinuteUtc = DateTime.UtcNow;
        private TimeSpan _forcedOffset;


        public DateTime CurrentDateTimeLocal => CurrentDateTimeUtc.ToLocalTime();
        public DateTime CurrentDateTimeMinuteLocal => CurrentDateTimeUtc.ToLocalTime();

        public DateTime CurrentDateTimeConvention => CurrentDateTimeUtc.AddHours(2);
        public DateTime CurrentDateTimeMinuteConvention => CurrentDateTimeUtc.AddHours(2);


        public DateTime CurrentDateTimeUtc
        {
            get
            {
                return _currentDateTimeUtc + ForcedOffset;
            }
            private set
            {
                if (!SetProperty(ref _currentDateTimeUtc, value)) return;

                OnPropertyChanged(nameof(CurrentDateTimeLocal));
                OnPropertyChanged(nameof(CurrentDateTimeConvention));

                CurrentDateTimeMinuteUtc = _currentDateTimeUtc.
                    AddTicks(-(CurrentDateTimeUtc.Ticks%TimeSpan.TicksPerMinute));
            }
        }

        public DateTime CurrentDateTimeMinuteUtc
        {
            get
            {
                return _currentDateTimeMinuteUtc + ForcedOffset;
            }
            private set
            {
                if (!SetProperty(ref _currentDateTimeMinuteUtc, value)) return;

                OnPropertyChanged(nameof(CurrentDateTimeMinuteLocal));
                OnPropertyChanged(nameof(CurrentDateTimeMinuteConvention));
            }
        }

        public TimeSpan ForcedOffset
        {
            get { return _forcedOffset; }
            set {
                _forcedOffset = value;
                OnPropertyChanged(nameof(CurrentDateTimeUtc));
                OnPropertyChanged(nameof(CurrentDateTimeMinuteUtc));
                OnPropertyChanged(nameof(CurrentDateTimeLocal));
                OnPropertyChanged(nameof(CurrentDateTimeMinuteLocal));
                OnPropertyChanged(nameof(CurrentDateTimeConvention));
                OnPropertyChanged(nameof(CurrentDateTimeMinuteConvention));
            }
        }


        public TimeProvider()
        {
            InitializeDispatcherFromCurrentThread();

            ForcedOffset = TimeSpan.Zero;

            //if (DesignMode.DesignModeEnabled) {
                ForcedOffset = new DateTime(2017, 08, 16, 14, 55, 00, DateTimeKind.Utc) - DateTime.UtcNow;
            //}

            var updateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            updateTimer.Tick += _updateTimer_Tick;
            updateTimer.Start();
        }

        private void _updateTimer_Tick(object sender, object e)
        {
            CurrentDateTimeUtc = DateTime.UtcNow;
        }
    }
}
