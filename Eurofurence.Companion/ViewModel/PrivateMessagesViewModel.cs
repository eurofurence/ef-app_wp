using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System;
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

        public ICommand MarkMessageAsReadCommand { get; }

        public PrivateMessagesViewModel(IPrivateMessagesViewModelContext privateMessagesViewModelContext)
        {
            _privateMessagesViewModelContext = privateMessagesViewModelContext;

            _privateMessagesViewModelContext.WatchProperty(nameof(HasMessages), _ => OnPropertyChanged(nameof(HasMessages)));
            _privateMessagesViewModelContext.WatchProperty(nameof(HasUnreadMessages), _ => OnPropertyChanged(nameof(HasUnreadMessages)));
            _privateMessagesViewModelContext.WatchProperty(nameof(UnreadMessagesCount), _ => OnPropertyChanged(nameof(UnreadMessagesCount)));

            MarkMessageAsReadCommand = new RelayCommand(async (p) =>
                await _privateMessagesViewModelContext.MarkPrivateMessageAsReadAsync((PrivateMessageViewModel)p));
        }
    }
}
