using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Ninject;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Eurofurence.Companion.Views.Controls
{
    public sealed partial class DebugControl : UserControl
    {
        private ITimeProvider _timeProvider;

        public DebugControl()
        {
            this.InitializeComponent();
            _timeProvider = ServiceLocator.Current.TimeProvider;
        }

        private void SetConLocalTime(int year, int month, int day, int hour, int minute, int second)
        {
            _timeProvider.ForcedOffset = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc)
                - DateTime.UtcNow - TimeSpan.FromHours(2); // 2h as EF will be in UTC+02:00
        }

        private void SetTime_ConStart(object sender, TappedRoutedEventArgs e)
        {
            SetConLocalTime(2017, 08, 16, 9, 0, 0);
        }

        private void SetTime_AddDay(object sender, TappedRoutedEventArgs e)
        {
            _timeProvider.ForcedOffset += TimeSpan.FromDays(1);
        }
        private void SetTime_AddHour(object sender, TappedRoutedEventArgs e)
        {
            _timeProvider.ForcedOffset += TimeSpan.FromHours(1);
        }

        private void SetTime_AddQuarterHour(object sender, TappedRoutedEventArgs e)
        {
            _timeProvider.ForcedOffset += TimeSpan.FromMinutes(15);
        }
        private void SetTime_AddMinute(object sender, TappedRoutedEventArgs e)
        {
            _timeProvider.ForcedOffset += TimeSpan.FromMinutes(1);
        }

        private void SetTime_RemoveDay(object sender, TappedRoutedEventArgs e)
        {
            _timeProvider.ForcedOffset -= TimeSpan.FromDays(1);
        }
        private void SetTime_RemoveHour(object sender, TappedRoutedEventArgs e)
        {
            _timeProvider.ForcedOffset -= TimeSpan.FromHours(1);
        }
        private void SetTime_RemoveMinute(object sender, TappedRoutedEventArgs e)
        {
            _timeProvider.ForcedOffset -= TimeSpan.FromMinutes(1);
        }

        private void SetTime_EndOfMinute(object sender, TappedRoutedEventArgs e)
        {
            var secondsToAdd = 55 - _timeProvider.CurrentDateTimeUtc.Second;
            _timeProvider.ForcedOffset += TimeSpan.FromSeconds(secondsToAdd);
        }
    }
}
