using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class PrivateMessageViewModel : BindableBase
    {
        public PrivateMessage Entity { get; private set; }

        public bool IsRead => Entity.ReadDateTimeUtc.HasValue;

        public PrivateMessageViewModel(PrivateMessage entity)
        {
            Entity = entity;
        }

        private void Invalidate()
        {
            OnPropertyChanged(nameof(Entity));
            OnPropertyChanged(nameof(IsRead));
        }

        public void Update(PrivateMessage message)
        {
            Entity = message;
            Invalidate();
        }
    }
}
