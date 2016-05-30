using Eurofurence.Companion.DependencyResolution;
using System;
using Windows.UI.Xaml;
using Eurofurence.Companion.Common.Abstractions;

namespace Eurofurence.Companion.Common
{
    [IocBeacon(TargetType = typeof(ITimeProvider), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class TimeProvider : BindableBase, ITimeProvider
    {
        private DateTime _currentDateTimeUtc = DateTime.UtcNow;
        private DateTime _currentDateTimeMinuteUtc = DateTime.UtcNow;
        private TimeSpan _forcedOffset;

        public DateTime CurrentDateTimeUtc
        {
            get
            {
                return _currentDateTimeUtc + ForcedOffset;
            }
            private set
            {
                SetProperty(ref _currentDateTimeUtc, value);
                CurrentDateTimeMinuteUtc = _currentDateTimeUtc.
                    AddTicks(-(CurrentDateTimeUtc.Ticks % TimeSpan.TicksPerMinute));
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
                SetProperty(ref _currentDateTimeMinuteUtc, value);
            }
        }

        public TimeSpan ForcedOffset
        {
            get { return _forcedOffset; }
            set {
                _forcedOffset = value;
                OnPropertyChanged(nameof(CurrentDateTimeUtc));
                OnPropertyChanged(nameof(CurrentDateTimeMinuteUtc));
            }
        }


        public TimeProvider()
        {
            InitializeDispatcherFromCurrentThread();

            ForcedOffset = TimeSpan.Zero;
            //ForcedOffset = new DateTime(2015, 08, 19, 16, 45, 00) - DateTime.UtcNow;
            ForcedOffset = new DateTime(2015, 08, 19, 16, 59, 45, DateTimeKind.Utc) - DateTime.UtcNow;
            //ForcedOffset = new DateTime(2015, 08, 20, 06, 59, 45, DateTimeKind.Utc) - DateTime.UtcNow;

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
