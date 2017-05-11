using Windows.ApplicationModel.Background;
using Windows.Networking.PushNotifications;

namespace Eurofurence.Companion.PushHandlerBackgroundTask
{
    public sealed class TaskImplementation : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var x = taskInstance.GetDeferral();

            var rawNotification = (taskInstance.TriggerDetails as RawNotification);

            if (rawNotification != null)
            {
                var handler = new NotificationHandler(false);
                handler.HandleRawNotification(rawNotification.Content);
            }

            x.Complete();
        }
    }
}
