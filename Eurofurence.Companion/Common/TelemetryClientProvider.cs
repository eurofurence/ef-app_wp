using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using Eurofurence.Companion.Common.Abstractions;

namespace Eurofurence.Companion.Common
{
    [IocBeacon(TargetType = typeof(ITelemetryClientProvider), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class TelemetryClientProvider : ITelemetryClientProvider
    {
        private readonly PersistenceChannel _persistenceChannel;
        private readonly ApplicationSettingsContext _applicationSettingsContext;

        public TelemetryClient Client { get; }

        public TelemetryClientProvider(ApplicationSettingsContext applicationSettingsContext)
        {
            _applicationSettingsContext = applicationSettingsContext;

            _persistenceChannel = new PersistenceChannel
            {
                DeveloperMode = false, // Debugger.IsAttached,
                SendingInterval = TimeSpan.FromSeconds(10)
            };

            Client = new TelemetryClient(
                new TelemetryConfiguration
                {
                    TelemetryChannel = _persistenceChannel
                }
                ) {InstrumentationKey = "83692601-c84e-47a0-8e64-4db964e40980"};

            Client.Context.Session.Id = $"{Guid.NewGuid()}";
            Client.Context.User.Id = $"{_applicationSettingsContext.UniqueRandomUserId}";

        }
    }
}
