using Microsoft.ApplicationInsights;

namespace Eurofurence.Companion.Common
{
    public interface ITelemetryClientProvider
    {
        TelemetryClient Client { get; }
    }
}