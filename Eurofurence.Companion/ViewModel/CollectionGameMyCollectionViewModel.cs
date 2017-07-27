using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api.CollectingGame;
using Eurofurence.Companion.Services;

namespace Eurofurence.Companion.ViewModel
{
    public class CollectionGameMyCollectionViewModel : BindableBase
    {
        private readonly CollectingGameService _service;

        private bool _isBusy = false;

        public ObservableCollection<PlayerCollectionEntry> PlayerCollectionEntries { get; set; }


        public bool IsBusy { get { return _isBusy; } set { SetProperty(ref _isBusy, value); } }

        public ICommand Load { get; }

        public CollectionGameMyCollectionViewModel(CollectingGameService service)
        {
            _service = service;
            PlayerCollectionEntries = new ObservableCollection<PlayerCollectionEntry>();
            Load = new AwaitableCommand(LoadAsync, (e, t) => AwaitableCommandExceptionHandlerFactory.RetryOrReturnToMainPage(e, t));
        }

        private async Task RefreshAsync()
        {
            var entries = await _service.GetPlayerCollectionEntriesAsync();

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                PlayerCollectionEntries.Clear();
                foreach (var entry in entries) PlayerCollectionEntries.Add(entry);
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