using Newtonsoft.Json.Linq;
using NotificationsExtensions.ToastContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Eurofurence.Companion.PushHandlerBackgroundTask
{
    public sealed class NotificationHandler
    {
        private readonly bool _isApplicationRunning;
        private readonly ToastNotifier _toastNotificationManager;

        public NotificationHandler(bool isApplicationRunning)
        {
            _isApplicationRunning = isApplicationRunning;
            _toastNotificationManager = ToastNotificationManager.CreateToastNotifier();
        }

        public void HandleRawNotification(string payload)
        {
            if (payload.Equals("privatemessage_received", StringComparison.CurrentCultureIgnoreCase))
            {
                PrivateMessageReceived();
                return;
            }

            if (!payload.StartsWith("{")) return;

            try
            {

                var wrapper = new
                {
                    Event = "",
                    Content = new JObject()
                };

                var content = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(payload, wrapper);


                if (content?.Event.Equals("NewAnnouncement", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    var announcement = content.Content.ToObject<DataModel.Api.Announcement>();

                    // Already expired or not ready yet - we won't toast that.
                    if (announcement.ValidUntilDateTimeUtc < DateTime.UtcNow || announcement.ValidFromDateTimeUtc > DateTime.UtcNow) return;



                    //var showOnDateTimeUtc = announcement.ValidFromDateTimeUtc < DateTime.UtcNow ? DateTime.UtcNow : announcement.ValidFromDateTimeUtc;

                    var parts = SplitGuid(announcement.Id);

                    var toast = new ScheduledToastNotification(
                        CreateToast(announcement.Title, announcement.Content,
                            $"toast:announcementDetail:{announcement.Id}"),
                        DateTime.UtcNow.AddSeconds(1))
                    {
                        Id = parts.Item1,
                        Tag = parts.Item2,
                        Group = $"a:push",
                    };

                    _toastNotificationManager.AddToSchedule(toast);
                }
            }
            catch (Exception)
            {

            }
        }

        private void PrivateMessageReceived()
        {
            var payload = CreateToast("You have received a personal message!", "Open the Eurofurence app to read it.", "");
            var toast = new ScheduledToastNotification(payload, DateTime.UtcNow.AddSeconds(1))
            {
                Id = new Random().Next(0, 100000).ToString()
            };

            _toastNotificationManager.AddToSchedule(toast);
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

        private Tuple<string, string> SplitGuid(Guid id)
        {
            var bytes = id.ToByteArray();
            return new Tuple<string, string>(Convert.ToBase64String(bytes, 8, 8), Convert.ToBase64String(bytes, 0, 8));
        }

    }
}
 