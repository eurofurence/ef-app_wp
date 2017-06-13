using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Eurofurence.Companion.ViewModel
{
    public class PrivateMessagesViewModel: BindableBase
    {
        private readonly IPrivateMessagesViewModelContext _privateMessagesViewModelContext;

        public ObservableCollection<PrivateMessageViewModel> Messages => _privateMessagesViewModelContext.Messages;
        public bool HasMessages => _privateMessagesViewModelContext.HasMessages;
        public bool HasUnreadMessages => _privateMessagesViewModelContext.HasUnreadMessages;
        public int UnreadMessagesCount => _privateMessagesViewModelContext.UnreadMessagesCount;

        public string NotificationText
        {
            get
            {
                if (HasUnreadMessages) return string.Format(Translations.PrivateMessages_HasUnread, UnreadMessagesCount, Messages.Count);
                if (HasMessages) return string.Format(Translations.PrivateMessages_HasRead, Messages.Count);

                return Translations.PrivateMessages_NoMessages;
            }
        }

        public ICommand MarkMessageAsReadCommand { get; }

        public PrivateMessagesViewModel(IPrivateMessagesViewModelContext privateMessagesViewModelContext)
        {
            _privateMessagesViewModelContext = privateMessagesViewModelContext;

            _privateMessagesViewModelContext.Invalidated += (s, e) =>
            {
                OnPropertyChanged(nameof(HasMessages));
                OnPropertyChanged(nameof(HasUnreadMessages));
                OnPropertyChanged(nameof(UnreadMessagesCount));
                OnPropertyChanged(nameof(NotificationText));
            };
            
            MarkMessageAsReadCommand = new RelayCommand(async (p) =>
                await _privateMessagesViewModelContext.MarkPrivateMessageAsReadAsync((PrivateMessageViewModel)p));
        }
    }
}
