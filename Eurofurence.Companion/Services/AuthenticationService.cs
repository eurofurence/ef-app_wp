using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using System;
using System.Threading.Tasks;

namespace Eurofurence.Companion.Services
{
    [IocBeacon(TargetType = typeof(AuthenticationService), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class AuthenticationService
    {
        public class AuthenticationState
        {
            public bool IsAuthenticated { get; set; }

            public string Username { get; set; }
            public string Token { get; set; }
            public DateTime TokenExpiration { get; set; }

        }

        public AuthenticationState State { get; private set; }


        private readonly ApplicationSettingsContext _applicationSettingsContext;
        private readonly EurofurenceWebApiClient _apiClient;
        private readonly ApplicationSettingsManager _applicationSettingsManager;

        public event EventHandler AuthenticationStateChanged;



        public AuthenticationService(ApplicationSettingsManager applicationSettingsManager)
        {
            _apiClient = new EurofurenceWebApiClient(Consts.WEB_API_ENDPOINT_URL);
            _applicationSettingsManager = applicationSettingsManager;

            State = _applicationSettingsManager.Get<AuthenticationState>("AuthenticationState", new AuthenticationState() { IsAuthenticated = false });
        }

        public async Task LogoutAsync()
        {
            await Task.Delay(0);
            State = new AuthenticationState()
            {
                IsAuthenticated = false
            };

            _applicationSettingsManager.Set("AuthenticationState", State);  
            AuthenticationStateChanged?.Invoke(this, null);
        }

        public async Task<bool> LoginAsync(int regNo, string username, string password)
        {
            var response = await _apiClient.PostAsync<RegSysAuthenticationRequest, AuthenticationResponse>(
                "Tokens/RegSys",
                new RegSysAuthenticationRequest()
                {
                    RegNo = regNo,
                    Username = username,
                    Password = password
                });

            if (response.IsSuccessful)
            {
                State = new AuthenticationState()
                {
                    IsAuthenticated = true,
                    Username = response.Username,
                    TokenExpiration = response.TokenValidUntil,
                    Token = response.Token
                };
            }
            else
            {
                State = new AuthenticationState()
                {
                    IsAuthenticated = false
                };
            }

            _applicationSettingsManager.Set("AuthenticationState", State);
            AuthenticationStateChanged?.Invoke(this, null);

            return State.IsAuthenticated;
        }
    }
}
