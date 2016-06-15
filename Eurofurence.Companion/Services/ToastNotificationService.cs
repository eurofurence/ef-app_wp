using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Data.Xml.Dom;
using Windows.Phone.Notification.Management;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Windows.UI.Notifications;
using NotificationsExtensions.TileContent;
using NotificationsExtensions.ToastContent;

namespace Eurofurence.Companion.Services
{
    [IocBeacon(TargetType = typeof(ToastNotificationService), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class ToastNotificationService
    {
        private readonly IDataContext _dataContext;
        private readonly ToastNotifier _toastNotificationManager;

        public ToastNotificationService(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _toastNotificationManager = ToastNotificationManager.CreateToastNotifier();

            UpdateEventNotifications();
        }

        private void UpdateEventNotifications()
        {
            var toasts = _toastNotificationManager
                .GetScheduledToastNotifications()
                .Where(a => a.Group == "efapp_tg_1");

            foreach (var toast in toasts.Where(a => a.Group == "efapp_tg_1"))
            {
                _toastNotificationManager.RemoveFromSchedule(toast);
            }
            

            foreach (var @event in _dataContext.EventEntries.Where(a => a.AttributesProxy.Extension.IsFavorite))
            {
                var toast = new ScheduledToastNotification(
                    CreateToast(
                        $"{@event.Title} {@event.SubTitle}", 
                        "Event starts in 30 minutes",
                        $"toast:eventDetail:{@event.Id}"
                        ),
                    //@event.EventDateTimeUtc.AddMinutes(-30)
                    DateTime.UtcNow.AddSeconds(5)
                    )
                {
                    Id = GetRandomToastId(),
                    Group = "efapp_tg_1",
                };

                _toastNotificationManager.AddToSchedule(toast);
            }
        }

        private XmlDocument CreateToast(string title, string message, string launchUrl)
        {
            var toast = ToastContentFactory.CreateToastText02();

            toast.TextHeading.Text = title;
            toast.TextBodyWrap.Text = message;
            toast.Duration = ToastDuration.Long;
            toast.Launch = launchUrl;

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(toast.GetXml().ToString());

            return xmlDocument;
        }

        private string GetRandomToastId()
        {
            return Math.Floor(new Random().NextDouble()*1000000).ToString();
        }



    }
}
