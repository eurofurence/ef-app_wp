using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api.CollectingGame;
using Eurofurence.Companion.Services;
using Eurofurence.Companion.ViewModel.Local;

namespace Eurofurence.Companion.ViewModel
{
    public class CollectionGamePlayerViewModel : BindableBase
    {
        private readonly AuthenticationViewModel _authenticationViewModel;
        private readonly CollectingGameService _service;
        private readonly NavigationViewModel _navigationViewModel;
        private readonly INavigationMediator _navigationMediator;


        private int _pageIndex = 0;
        private bool _isBusy = false;
        private string _tokenValue = string.Empty;
        private string _errorMessage = string.Empty;

        public PlayerParticipationInfo ParticipationInfo { get; set; }
        public CollectTokenResponse Response { get; set; }

        public int PageIndex { get { return _pageIndex; } set { SetProperty(ref _pageIndex, value); } }

        public bool IsBusy { get { return _isBusy; } set { SetProperty(ref _isBusy, value); } }
        public string TokenValue { get { return _tokenValue; } set { SetProperty(ref _tokenValue, value); } }
        public string ErrorMessage { get { return _errorMessage; } set { SetProperty(ref _errorMessage, value); } }

        public string TextTitle => $"Hey, {ParticipationInfo?.Name ?? ""}!";

        public string TextHeader
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append($"You've already caught {ParticipationInfo?.CollectionCount ?? 0} fursuits!");

                return sb.ToString();
            }  
        } 


        public ICommand Load { get; }
        public ICommand SubmitTokenCommand { get; }
        public ICommand BackCommand { get; }


        public CollectionGamePlayerViewModel(
            AuthenticationViewModel authenticationViewModel,
            CollectingGameService service,
            NavigationViewModel navigationViewModel,
            INavigationMediator navigationMediator
            )
        {
            _authenticationViewModel = authenticationViewModel;
            _service = service;
            _navigationViewModel = navigationViewModel;
            _navigationMediator = navigationMediator;

            Load = new AwaitableCommand(LoadAsync, (e, t) => AwaitableCommandExceptionHandlerFactory.RetryOrReturnToMainPage(e, t));

            BackCommand = new AwaitableCommand(() => LoadAsync().ContinueWith(_ =>
            {
                ErrorMessage = string.Empty;
                PageIndex = 0;
                TokenValue = string.Empty;
            }));

            SubmitTokenCommand = new AwaitableCommand(SubmitTokenAsync, (e, t) => AwaitableCommandExceptionHandlerFactory.RetryOrReturnToMainPage(e, t));
        }


        private async Task SubmitTokenAsync()
        {
            IsBusy = true;
            var response = await _service.CollectAsync(TokenValue);

            if (!response.IsSuccessful)
            {
                ErrorMessage = response.ErrorMessage;
                await RefreshAsync();
            }
            else
            {
                Response = response.Value;
                OnPropertyChanged(nameof(Response));
                PageIndex = 1;
            }

            IsBusy = false;
        }

        private async Task RefreshAsync()
        {
            if (!_authenticationViewModel.IsAuthenticated)
            {

                await new MessageDialog(
                    "You must be logged on to play.\n\nWould you like to login now?",
                    "Login Required")
                {
                    Commands =
                    {
                        new UICommand("Cancel", _ => _navigationMediator.NavigateAsync(typeof(Views.MainPage), forceNewStack: true)),
                        new UICommand("Login", _ => _navigationMediator.NavigateAsync(typeof(Views.UserCentralPage), new Action(
                            () => { _navigationMediator.NavigateAsync(typeof(Views.CollectionGamePlayerView), forceNewStack: true); }))),
                    },
                    DefaultCommandIndex = 1
                }.ShowAsync();

                return;
            }

            ParticipationInfo = await _service.GetPlayerParticipationInfoAsync(); ;

            OnPropertyChanged(nameof(ParticipationInfo));
            OnPropertyChanged(nameof(TextTitle));
            OnPropertyChanged(nameof(TextHeader));
        }

        private async Task LoadAsync()
        {
            IsBusy = true;
            await RefreshAsync();
            IsBusy = false;
        }
    }
}