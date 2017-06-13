using System.Collections.ObjectModel;
using Eurofurence.Companion.ViewModel.Local.Entity;
using System.ComponentModel;
using System.Threading.Tasks;
using System;

namespace Eurofurence.Companion.ViewModel.Abstractions
{
    public interface IPrivateMessagesViewModelContext
    {
        bool HasMessages { get; }
        bool HasUnreadMessages { get; }
        int UnreadMessagesCount { get;}
        ObservableCollection<PrivateMessageViewModel> Messages { get; }

        Task MarkPrivateMessageAsReadAsync(PrivateMessageViewModel message);

        event EventHandler Invalidated;
    }
}