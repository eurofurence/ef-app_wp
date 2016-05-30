using Microsoft.ApplicationInsights;

namespace Eurofurence.Companion.Common.Abstractions
{
    public interface ITelemetryClientProvider
    {
        TelemetryClient Client { get; }
    }
}