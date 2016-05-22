using Eurofurence.Companion.DependencyResolution;
using System;
using Windows.UI.Xaml;

namespace Eurofurence.Companion.Common
{
    [IocBeacon(TargetType = typeof(TimeProvider), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class TimeProvider : BindableBase
    {
        private DateTime _currentDateTimeLocal = DateTime.Now;
        private DispatcherTimer _updateTimer;

        public DateTime CurrentDateTimeLocal
        {
            get
            {
                return _currentDateTimeLocal + ForcedOffset;
            }
            private set
            {
                SetProperty(ref _currentDateTimeLocal, value);
            }
        }

        public TimeSpan ForcedOffset { get; set; }
        

        public TimeProvider()
        {
            InitializeDispatcherFromCurrentThread();

            ForcedOffset = TimeSpan.Zero;
            ForcedOffset = new DateTime(2015, 08, 19, 16, 45, 00) - DateTime.Now;

            _updateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _updateTimer.Tick += _updateTimer_Tick;
            _updateTimer.Start();
        }

        private void _updateTimer_Tick(object sender, object e)
        {
            CurrentDateTimeLocal = DateTime.Now;
        }
    }
}
