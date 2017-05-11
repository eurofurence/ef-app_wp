using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Eurofurence.Companion.ViewModel.Local
{
    [IocBeacon(TargetType = typeof(AuthenticationViewModel))]
    public class AuthenticationViewModel : BindableBase
    {

        public bool IsAuthenticated => _authenticationService.State.IsAuthenticated;
        public string Username => _authenticationService.State.Username;
        public DateTime TokenExpiration => _authenticationService.State.TokenExpiration;


        private bool _isBusy = false;
        public bool IsBusy { get { return _isBusy; } set { SetProperty(ref _isBusy, value); } }

        public bool HasErrorMessage => !string.IsNullOrWhiteSpace(_errorMessage);

        private string _errorMessage;
        public string ErrorMessage {
            get { return _errorMessage; }
            set {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public string RequestRegNo { get; set; }
        public string RequestUsername { get; set; }
        public string RequestPassword { get; set; }


        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }


        private readonly AuthenticationService _authenticationService;

        public AuthenticationViewModel(AuthenticationService authenticationService)
        {
            InitializeDispatcherFromCurrentThread();
            _authenticationService = authenticationService;

            _authenticationService.AuthenticationStateChanged += _authenticationService_AuthenticationStateChanged;

            LoginCommand = new AwaitableCommand(LoginAsync);
            LogoutCommand = new AwaitableCommand(_authenticationService.LogoutAsync);
        }


        private async Task LoginAsync()
        {
            IsBusy = true;
            ErrorMessage = "";
            try
            {
                int numericRegNo = 0;
                if (!int.TryParse(this.RequestRegNo, out numericRegNo) || numericRegNo <= 0)
                {
                    ErrorMessage = Translations.Login_Error_RegistrationNumberInvalid;
                    return;
                }

                if (string.IsNullOrWhiteSpace(RequestUsername) ||string.IsNullOrWhiteSpace(RequestPassword))
                {
                    ErrorMessage = Translations.Login_Error_UsernamePasswordNotProvided;
                    return;
                }

                var isSuccessful = 
                    await _authenticationService.LoginAsync(numericRegNo, this.RequestUsername, this.RequestPassword);

                if (!isSuccessful)
                {
                    ErrorMessage = Translations.Login_Error_CredentialsRejected;
                }
            }
            finally {
                IsBusy = false;
            }
        }

        private void _authenticationService_AuthenticationStateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsAuthenticated));
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(TokenExpiration));
        }
    }
}
