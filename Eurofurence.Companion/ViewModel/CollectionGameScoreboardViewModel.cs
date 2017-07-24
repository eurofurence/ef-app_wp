using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api.CollectingGame;
using Eurofurence.Companion.Services;
using Eurofurence.Companion.ViewModel.Local;

namespace Eurofurence.Companion.ViewModel
{
    public class CollectionGameScoreboardViewModel : BindableBase
    {
        private readonly CollectingGameService _service;

        private bool _isBusy = false;
        private NavigationViewModel _navigationViewModel;

        public ObservableCollection<PlayerScoreboardEntry> PlayerScoreboardEntries { get; set; }
        public ObservableCollection<FursuitScoreboardEntry> FursuitScoreboardEntries { get; set; }


        public bool IsBusy { get { return _isBusy; } set { SetProperty(ref _isBusy, value); } }

        public ICommand Load { get; }

        public CollectionGameScoreboardViewModel(
            CollectingGameService service,
            NavigationViewModel navigationViewModel)
        {
            _service = service;
            _navigationViewModel = navigationViewModel;

            PlayerScoreboardEntries = new ObservableCollection<PlayerScoreboardEntry>();
            FursuitScoreboardEntries = new ObservableCollection<FursuitScoreboardEntry>();

            Load = new AwaitableCommand(LoadAsync, (e, t) => AwaitableCommandExceptionHandlerFactory.RetryOrReturnToMainPage(e, t));
        }


        private async Task RefreshAsync()
        {
            var players = await _service.GetPlayerScoreboardAsync();
            var fursuits = await _service.GetFursuitScoreboardAsync();

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PlayerScoreboardEntries.Clear();
                FursuitScoreboardEntries.Clear();

                foreach (var player in players) PlayerScoreboardEntries.Add(player);
                foreach (var fursuit in fursuits) FursuitScoreboardEntries.Add(fursuit);
            });
        }

        private async Task LoadAsync()
        {
            IsBusy = true;

            await RefreshAsync();
            IsBusy = false;
        }
    }
}