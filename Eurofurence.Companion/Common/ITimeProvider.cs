using System;
using System.ComponentModel;

namespace Eurofurence.Companion.Common
{
    public interface ITimeProvider: INotifyPropertyChanged
    {
        DateTime CurrentDateTimeLocal { get; }
        DateTime CurrentDateTimeMinuteLocal { get; }
        TimeSpan ForcedOffset { get; set; }
    }
}