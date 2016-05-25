using System;
using System.ComponentModel;

namespace Eurofurence.Companion.Common
{
    public static class PropertyWatcherExtensions
    {
        public static void WatchProperty(this INotifyPropertyChanged target, string propertyName,
            Action<PropertyChangedEventArgs> action)
        {
            target.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals(propertyName))
                {
                    action(args);
                }
                ;
            };
        }
    }
}
