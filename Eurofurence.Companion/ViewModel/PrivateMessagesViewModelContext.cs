using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.Services;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Core;

namespace Eurofurence.Companion.ViewModel
{

    [IocBeacon(TargetType = typeof(IPrivateMessagesViewModelContext), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class PrivateMessagesViewModelContext : BindableBase, IPrivateMessagesViewModelContext
    {
        private readonly PrivateMessageService _privateMessageService;

        public ObservableCollection<PrivateMessageViewModel> Messages { get; }
        public bool HasUnreadMessages => Messages.Any(a => !a.IsRead);
        public bool HasMessages => Messages.Count > 0;
        public int UnreadMessagesCount => Messages.Count(a => !a.IsRead);

        public event EventHandler Invalidated;


        public PrivateMessagesViewModelContext(PrivateMessageService privateMessageService)
        {
            _privateMessageService = privateMessageService;
            _privateMessageService.Updated += async (s, e) => await UpdateAsync();

            Messages = new ObservableCollection<PrivateMessageViewModel>();

            if (DesignMode.DesignModeEnabled)
            {
                CreateMockData();
            }
            else
            {
                Task.Run(async () => await UpdateAsync());
            }
        }

        private void CreateMockData()
        {
            for (int i = 0; i < 3; i++)
            {
                Messages.Add(new PrivateMessageViewModel(new PrivateMessage()
                {
                    AuthorName = $"Author-{i}",
                    CreatedDateTimeUtc = DateTime.UtcNow.AddMinutes(i * 7),
                    ReceivedDateTimeUtc = DateTime.UtcNow.AddMinutes(i * 7).AddSeconds(15),
                    ReadDateTimeUtc = i % 2 == 0 ? (DateTime?)DateTime.UtcNow.AddMinutes(i * 15) : null,
                    Id = Guid.NewGuid(),
                    Message = $"Message-{i}",
                    Subject = $"Subject-{i}"
                }));
            }
        }

        private async Task UpdateAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var source = _privateMessageService.Messages;

                var messagesToAdd = source.Where(s => !Messages.Any(l => l.Entity.Id == s.Id))
                    .OrderBy(a => a.CreatedDateTimeUtc)
                    .ToList();
                var messagesToUpdate = source.Where(s => Messages.Any(l => l.Entity.Id == s.Id)).ToList();

                foreach (var message in Messages.Where(l => !source.Any(s => s.Id == l.Entity.Id)).ToList())
                    Messages.Remove(message);

                foreach (var message in messagesToAdd)
                    Messages.Insert(0, new PrivateMessageViewModel(message));

                foreach (var message in messagesToUpdate)
                {
                    var target = Messages.SingleOrDefault(l => l.Entity.Id == message.Id);
                    target.Update(message);
                }

                Invalidated?.Invoke(this, null);
            });
        }

        public Task MarkPrivateMessageAsReadAsync(PrivateMessageViewModel message)
        {
            return _privateMessageService.MarkPrivateMessageAsReadAsync(message.Entity);
        }
    }
}
