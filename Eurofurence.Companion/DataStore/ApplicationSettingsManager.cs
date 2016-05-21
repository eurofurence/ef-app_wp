using System;
using Windows.Storage;
using Newtonsoft.Json;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.DataStore
{
    [IocBeacon(TargetType = typeof(IApplicationSettingsManager))]
    public class ApplicationSettingsManager : IApplicationSettingsManager
    {
        public T Get<T>(string key, T defaultValue)
        {
            try
            {
                var values = ApplicationData.Current.LocalSettings.Values;
                if (values.ContainsKey(key) && values[key] is string)
                {
                    return JsonConvert.DeserializeObject<T>((string) values[key]);
                }

                return defaultValue;
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public void Set<T>(string key, T value)
        {
            var values = ApplicationData.Current.LocalSettings.Values;
            values[key] = JsonConvert.SerializeObject(value);
            ApplicationData.Current.SignalDataChanged();
        }
    }
}