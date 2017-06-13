using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eurofurence.Companion.Services
{
    [IocBeacon(Scope = IocBeacon.ScopeEnum.Singleton, Environment = IocBeacon.EnvironmentEnum.Any, TargetType = typeof(PrivateMessageService))]
    public class PrivateMessageService
    {
        private readonly EurofurenceWebApiClient _apiClient;
        private readonly AuthenticationService _authenticationService;
        private List<PrivateMessage> _messages;

        public List<PrivateMessage> Messages => _messages;
        public event EventHandler Updated;

        public PrivateMessageService(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _apiClient = new EurofurenceWebApiClient(Consts.WEB_API_ENDPOINT_URL);
            _messages = new List<PrivateMessage>();

            _authenticationService.AuthenticationStateChanged += (s, e) => { Task.Run(QueryPrivateMessagesAsync); };
        }

        public async Task MarkPrivateMessageAsReadAsync(PrivateMessage message)
        {
            if (!_authenticationService.State.IsAuthenticated) return;

            try
            {
                var readDateTime = await _apiClient.PostAsync<object, DateTime>($"Communication/PrivateMessages/{message.Id}/Read", null,
                oAuthToken: _authenticationService.State.Token);

                message.ReadDateTimeUtc = readDateTime;

                Updated?.Invoke(this, null);
            }
            catch (Exception)
            {

            }
        }

        public async Task QueryPrivateMessagesAsync()
        {
            if (!_authenticationService.State.IsAuthenticated)
            {
                _messages.Clear();
                Updated?.Invoke(this, null);
                return;
            }

            try
            {
                _messages = (await _apiClient.GetAsync<PrivateMessage[]>("Communication/PrivateMessages",
                    oAuthToken: _authenticationService.State.Token)).ToList();
            }
            catch (Exception)
            {
                
            }

            Updated?.Invoke(this, null);
        }
    }
}
