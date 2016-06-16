using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataModel.Api;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;

namespace Eurofurence.Companion.Services
{
    [IocBeacon(TargetType = typeof(EventService))]
    public class EventService
    {
        private readonly IDataContext _dataContext;
        private readonly ToastNotificationService _toastNotificationService;

        public RelayCommand ToggleFavoriteStatusCommand { get; set; }

        public EventService(IDataContext dataContext, ToastNotificationService toastNotificationService)
        {
            _dataContext = dataContext;
            _toastNotificationService = toastNotificationService;

            ToggleFavoriteStatusCommand = new RelayCommand((p) => ToggleFavoriteStatus((EventEntry)p));
        }


        private void ToggleFavoriteStatus(EventEntry entity)
        {
            entity.AttributesProxy.Extension.IsFavorite = !entity.AttributesProxy.Extension.IsFavorite;

            if (entity.AttributesProxy.Extension.IsFavorite)
            {
                _toastNotificationService.QueueEventNotifications(entity);
            }
            else
            {
                _toastNotificationService.DequeueEventNotifications(entity);
            }
        }
    }
}
