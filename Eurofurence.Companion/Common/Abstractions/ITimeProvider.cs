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
        TimeSpan ForcedOffset { get; set; }
    }
}