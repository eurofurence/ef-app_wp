using Eurofurence.Companion.DependencyResolution;
using System;
using Windows.UI.Xaml;

namespace Eurofurence.Companion.Common
{
    [IocBeacon(TargetType = typeof(ITimeProvider), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class TimeProvider : BindableBase, ITimeProvider
    {
        private DateTime _currentDateTimeLocal = DateTime.Now;
        private DateTime _currentDateTimeMinuteLocal = DateTime.Now;

        public DateTime CurrentDateTimeLocal
        {
            get
            {
                return _currentDateTimeLocal + ForcedOffset;
            }
            private set
            {
                SetProperty(ref _currentDateTimeLocal, value);
                CurrentDateTimeMinuteLocal = CurrentDateTimeLocal.
                    AddTicks(-(CurrentDateTimeLocal.Ticks % TimeSpan.TicksPerMinute));
            }
        }

        public DateTime CurrentDateTimeMinuteLocal
        {
            get
            {
                return _currentDateTimeMinuteLocal + ForcedOffset;
            }
            private set
            {
                SetProperty(ref _currentDateTimeMinuteLocal, value);
            }
        }

        public TimeSpan ForcedOffset { get; set; }
        

        public TimeProvider()
        {
            InitializeDispatcherFromCurrentThread();

            ForcedOffset = TimeSpan.Zero;
            ForcedOffset = new DateTime(2015, 08, 19, 16, 45, 00) - DateTime.Now;

            var updateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            updateTimer.Tick += _updateTimer_Tick;
            updateTimer.Start();
        }

        private void _updateTimer_Tick(object sender, object e)
        {
            CurrentDateTimeLocal = DateTime.Now;
        }
    }
}
