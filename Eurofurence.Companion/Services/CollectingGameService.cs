using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api.CollectingGame;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.Services.Abstractions;
using System;
using System.Threading.Tasks;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.Services
{
    [IocBeacon(TargetType = typeof(CollectingGameService), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class CollectingGameService
    {
        private EurofurenceWebApiClient _apiClient;
        private AuthenticationService _authenticationService;
        private INetworkConnectivityService _networkConnectivityService;

        public CollectingGameService(
            AuthenticationService authenticationService,
            INetworkConnectivityService networkConnectivityService
            )
        {
            _authenticationService = authenticationService;
            _networkConnectivityService = networkConnectivityService;
            _apiClient = new EurofurenceWebApiClient(Consts.WEB_API_ENDPOINT_URL);
        }

        public async Task<FursuitParticipationInfo[]> GetFursuitParticipationInfoAsync()
        {
            var result = await _apiClient.GetAsync<FursuitParticipationInfo[]>("Fursuits/CollectingGame/FursuitParticipation",
                oAuthToken: _authenticationService.State.Token);

            return result;
        }

        public async Task<PlayerParticipationInfo> GetPlayerParticipationInfoAsync()
        {
            var result = await _apiClient.GetAsync<PlayerParticipationInfo>("Fursuits/CollectingGame/PlayerParticipation",
                oAuthToken: _authenticationService.State.Token);

            return result;
        }

        public async Task<ApiResult<CollectTokenResponse>> CollectAsync(string tokenValue)
        {
            var result = await _apiClient.PostAsyncAcceptErrors<string, CollectTokenResponse>("Fursuits/CollectingGame/PlayerParticipation/CollectToken",
                tokenValue, oAuthToken: _authenticationService.State.Token);

            return result;
        }


        public async Task<ApiResult<object>> LinkAsync(Guid fursuitBadgeId, string tokenValue)
        {
            var result = await _apiClient.PostAsyncAcceptErrors<string, object>($"Fursuits/CollectingGame/FursuitParticpation/Badges/{fursuitBadgeId}/Token", 
                tokenValue, oAuthToken: _authenticationService.State.Token);

            return result;
        }

        public async Task<FursuitScoreboardEntry[]> GetFursuitScoreboardAsync()
        {
            var result = await _apiClient.GetAsync<FursuitScoreboardEntry[]>("Fursuits/CollectingGame/FursuitParticipation/Scoreboard",
                oAuthToken: _authenticationService.State.Token);

            return result;
        }
        public async Task<PlayerScoreboardEntry[]> GetPlayerScoreboardAsync()
        {
            var result = await _apiClient.GetAsync<PlayerScoreboardEntry[]>("Fursuits/CollectingGame/PlayerParticipation/Scoreboard",
                oAuthToken: _authenticationService.State.Token);

            return result;
        }

        public async Task<PlayerCollectionEntry[]> GetPlayerCollectionEntriesAsync()
        {
            var result = await _apiClient.GetAsync<PlayerCollectionEntry[]>("Fursuits/CollectingGame/PlayerParticipation/CollectionEntries",
                oAuthToken: _authenticationService.State.Token);

            return result;
        }
    }
}
