namespace Eurofurence.Companion.DataStore.Abstractions
{
    public interface IApplicationSettingsManager
    {
        T Get<T>(string key, T defaultValue);
        void Set<T>(string key, T value);
    }
}