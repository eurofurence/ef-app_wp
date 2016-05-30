using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Diagnostics;
using Eurofurence.Companion.Common.Abstractions;

namespace Eurofurence.Companion.Common
{
    [IocBeacon(TargetType = typeof(ITelemetryClientProvider), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class TelemetryClientProvider : ITelemetryClientProvider
    {
        private TelemetryClient _client;
        private PersistenceChannel _persistenceChannel;
        private ApplicationSettingsContext _applicationSettingsContext;

        public TelemetryClient Client => _client;

        public TelemetryClientProvider(ApplicationSettingsContext applicationSettingsContext)
        {
            _applicationSettingsContext = applicationSettingsContext;

            _persistenceChannel = new PersistenceChannel
            {
                DeveloperMode = false, // Debugger.IsAttached,
                SendingInterval = TimeSpan.FromSeconds(10)
            };

            _client = new TelemetryClient(
                new TelemetryConfiguration
                {
                    TelemetryChannel = _persistenceChannel
                }
            );

            _client.InstrumentationKey = "83692601-c84e-47a0-8e64-4db964e40980";
            _client.Context.Session.Id = $"{Guid.NewGuid()}";
            _client.Context.User.Id = $"{_applicationSettingsContext.UniqueRandomUserId}";

        }
    }
}
