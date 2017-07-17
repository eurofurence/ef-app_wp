﻿using Eurofurence.Companion.Common;
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

            Load = new AwaitableCommand(LoadAsync, (ex) => _navigationViewModel.NavigateToMainPage.Execute(null));
            Select = new RelayCommand(p => DoSelect((FursuitParticipationInfo)p));

            CancelTokenAssignmentCommand = new RelayCommand((p) => { ErrorMessage = string.Empty; PageIndex = 0; });
            SubmitTokenAssignmentCommand = new AwaitableCommand(LinkTokenAsync);

            FursuitParticipations = new ObservableCollection<FursuitParticipationInfo>();
        }

        private void DoSelect(FursuitParticipationInfo p)
        {
            SelectedBadge = p.Badge;
            TokenValue = string.Empty;
            ErrorMessage = string.Empty;
            PageIndex = 1;
        }

        private async Task LinkTokenAsync()
        {
            IsBusy = true;
            var result = await _service.LinkAsync(SelectedBadge.Id, TokenValue);

            if (result) {
                await RefreshAsync();
                PageIndex = 0;
            }  else
            {
                ErrorMessage = "The token you specified could not be linked to the fursuit.\n\nEither the token is invalid, or the fursuit is already linked to an existing token.\n\nEach token can only be linked to a fursuit once.";
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
            await Task.Delay(300); // Todo- Remove.
            await RefreshAsync();
            IsBusy = false;
        }



    }
}
