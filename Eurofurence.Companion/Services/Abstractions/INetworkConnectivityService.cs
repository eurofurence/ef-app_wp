using System;

namespace Eurofurence.Companion.Services.Abstractions
{
    public interface INetworkConnectivityService
    {
        bool HasInternetAccess { get; }

        event EventHandler<bool> NetworkStatusChanged;
    }
}