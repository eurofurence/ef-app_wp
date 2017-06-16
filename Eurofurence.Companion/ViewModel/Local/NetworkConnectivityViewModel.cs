using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.Services.Abstractions;

namespace Eurofurence.Companion.ViewModel.Local
{
    public class NetworkConnectivityViewModel : BindableBase
    {
        private readonly INetworkConnectivityService _networkConnectivityService;

        public bool HasInternetAccess => _networkConnectivityService.HasInternetAccess;

        public NetworkConnectivityViewModel(INetworkConnectivityService networkConnectivityService)
        {
            _networkConnectivityService = networkConnectivityService;
            _networkConnectivityService.NetworkStatusChanged += 
                (s, e) => OnPropertyChanged(nameof(HasInternetAccess));
        }
    }
}
