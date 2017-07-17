using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Eurofurence.Companion.Common;
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



        private int _pageIndex = 0;
        private bool _isBusy = false;
        private string _tokenValue = string.Empty;
        private string _errorMessage = string.Empty;



        public int CollectionCount { get; set; }
        public bool HasScoreboardRank { get; set; }
        public CollectTokenResponse Response { get; set; }


        public int PageIndex { get { return _pageIndex; } set { SetProperty(ref _pageIndex, value); } }

        public bool IsBusy { get { return _isBusy; } set { SetProperty(ref _isBusy, value); } }
        public string TokenValue { get { return _tokenValue; } set { SetProperty(ref _tokenValue, value); } }
        public string ErrorMessage { get { return _errorMessage; } set { SetProperty(ref _errorMessage, value); } }
        public string Username => _authenticationViewModel.Username;





        public ICommand Load { get; }
        public ICommand SubmitTokenCommand { get; }
        public ICommand BackCommand { get; }


        public CollectionGamePlayerViewModel(
            AuthenticationViewModel authenticationViewModel,
            CollectingGameService service,
            NavigationViewModel navigationViewModel)
        {
            _authenticationViewModel = authenticationViewModel;
            _service = service;
            _navigationViewModel = navigationViewModel;

            Load = new AwaitableCommand(LoadAsync, (ex) => _navigationViewModel.NavigateToMainPage.Execute(null));
            BackCommand = new AwaitableCommand(() => LoadAsync().ContinueWith(_ =>
            {
                ErrorMessage = string.Empty;
                PageIndex = 0;
                TokenValue = string.Empty;
            }));
            SubmitTokenCommand = new AwaitableCommand(SubmitTokenAsync);

        }


        private async Task SubmitTokenAsync()
        {
            IsBusy = true;
            var response = await _service.CollectAsync(TokenValue);

            if (!response.IsSuccessful)
            {
                ErrorMessage = response.FailureMessage;
                await RefreshAsync();
            }
            else
            {
                Response = response;
                OnPropertyChanged(nameof(Response));
                PageIndex = 1;
            }

            IsBusy = false;
        }

        private async Task RefreshAsync()
        {
            var info = await _service.GetPlayerParticipationInfoAsync();

            CollectionCount = info.CollectionCount;
            HasScoreboardRank = info.ScoreboardRank.HasValue;

            OnPropertyChanged(nameof(CollectionCount));
            OnPropertyChanged(nameof(HasScoreboardRank));
        }

        private async Task LoadAsync()
        {
            IsBusy = true;
            await Task.Delay(300); // Todo- Remove.
            await RefreshAsync();
            IsBusy = false;
        }



    }
}