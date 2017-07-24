using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api.CollectingGame;
using Eurofurence.Companion.Services;
using Eurofurence.Companion.ViewModel.Local;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace Eurofurence.Companion.ViewModel
{
    public class CollectionGameManageFursuitsViewModel : BindableBase
    {
        private readonly CollectingGameService _service;
        private readonly NavigationViewModel _navigationViewModel;


        private int _pageIndex = 0;
        private bool _isBusy = false;
        private FursuitBadgeRecord _selectedBadge;
        private string _tokenValue = string.Empty;
        private string _errorMessage = string.Empty;


        public int PageIndex { get { return _pageIndex; } set { SetProperty(ref _pageIndex, value); } }
        public bool IsBusy { get { return _isBusy; } set { SetProperty(ref _isBusy, value); } }
        public FursuitBadgeRecord SelectedBadge {  get { return _selectedBadge; }  set { SetProperty(ref _selectedBadge, value); } }
        public string TokenValue { get { return _tokenValue; } set { SetProperty(ref _tokenValue, value); } }
        public string ErrorMessage { get { return _errorMessage; } set { SetProperty(ref _errorMessage, value); } }


        public ObservableCollection<FursuitParticipationInfo> FursuitParticipations { get; set; }

        public ICommand Load { get; }

        public ICommand Select { get; }

        public ICommand CancelTokenAssignmentCommand { get; }
        public ICommand SubmitTokenAssignmentCommand { get; }


        public CollectionGameManageFursuitsViewModel(
            CollectingGameService service, 
            NavigationViewModel navigationViewModel)
        {
            _service = service;
            _navigationViewModel = navigationViewModel;


            Load = new AwaitableCommand(LoadAsync, (e, t) => AwaitableCommandExceptionHandlerFactory.RetryOrReturnToMainPage(e, t));
                
            Select = new RelayCommand(p => DoSelect((FursuitParticipationInfo)p));

            CancelTokenAssignmentCommand = new RelayCommand((p) => { ErrorMessage = string.Empty; PageIndex = 0; });
            SubmitTokenAssignmentCommand = new AwaitableCommand(LinkTokenAsync, (e, t) => AwaitableCommandExceptionHandlerFactory.RetryOrReturnToMainPage(e, t));

            FursuitParticipations = new ObservableCollection<FursuitParticipationInfo>();
        }

        private async void DoSelect(FursuitParticipationInfo p)
        {
            if (p.IsParticipating)
            {
                await new MessageDialog(
                        "This suit is already participating. There'll be a fancy page here some day with statistics.")
                    .ShowAsync();
                return;
            }

            SelectedBadge = p.Badge;
            TokenValue = string.Empty;
            ErrorMessage = string.Empty;
            PageIndex = 1;
        }

        private async Task LinkTokenAsync()
        {
            IsBusy = true;
            var result = await _service.LinkAsync(SelectedBadge.Id, TokenValue);

            if (result.IsSuccessful) {
                await RefreshAsync();
                PageIndex = 0;
            }  else
            {
                ErrorMessage = result.ErrorMessage;
            }

            IsBusy = false;
        }

        private async Task RefreshAsync()
        {
            FursuitParticipations.Clear();

            var records = await _service.GetFursuitParticipationInfoAsync();
            foreach (var record in records)
                FursuitParticipations.Add(record);
        }

        private async Task LoadAsync()
        {
            IsBusy = true;
            await RefreshAsync();
            IsBusy = false;
        }
    }
}
