using System;
using Windows.Networking.Connectivity;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.Services.Abstractions;

namespace Eurofurence.Companion.Services
{
    [IocBeacon(
        Environment = IocBeacon.EnvironmentEnum.Any, 
        Scope = IocBeacon.ScopeEnum.Singleton, 
        TargetType = typeof(INetworkConnectivityService)
        )]
    public class NetworkConnectivityService : INetworkConnectivityService
    {
        public bool HasInternetAccess { get; private set; }
        public event EventHandler<bool> NetworkStatusChanged;

        public NetworkConnectivityService()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            NetworkInformation_NetworkStatusChanged(this);
        }

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            bool hasInternetAccess = NetworkInformation.GetInternetConnectionProfile()?
                .GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

            if (HasInternetAccess == hasInternetAccess) return;

            HasInternetAccess = hasInternetAccess;
            NetworkStatusChanged?.Invoke(this, HasInternetAccess);
        }
    }
}
