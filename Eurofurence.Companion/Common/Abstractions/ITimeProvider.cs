using System;
using System.ComponentModel;

namespace Eurofurence.Companion.Common.Abstractions
{
    public interface ITimeProvider: INotifyPropertyChanged
    {
        DateTime CurrentDateTimeUtc { get; }
        DateTime CurrentDateTimeMinuteUtc { get; }
        DateTime CurrentDateTimeLocal { get; }
        DateTime CurrentDateTimeMinuteLocal { get; }
        DateTime CurrentDateTimeConvention { get; }
        DateTime CurrentDateTimeMinuteConvention { get; }
        TimeSpan ForcedOffset { get; set; }
    }
}