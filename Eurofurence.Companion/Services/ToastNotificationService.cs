using System;
using System.Linq;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.DataModel.Api;
using NotificationsExtensions.ToastContent;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Eurofurence.Companion.Services
{
    [IocBeacon(TargetType = typeof(ToastNotificationService), Scope = IocBeacon.ScopeEnum.Singleton)]
    public class ToastNotificationService
    {
        private readonly IDataContext _dataContext;
        private readonly ITimeProvider _timeProvider;
        private readonly ToastNotifier _toastNotificationManager;

        public ToastNotificationService(IDataContext dataContext, ITimeProvider timeProvider)
        {
            _dataContext = dataContext;
            _timeProvider = timeProvider;
            _toastNotificationManager = ToastNotificationManager.CreateToastNotifier();

            RescheduleAllEventNotifications();
        }


        private DateTime? GetSanitizedDeliveryDate(DateTime origin)
        {
            if (origin > _timeProvider.CurrentDateTimeUtc)
            {
                return origin.Add(TimeSpan.Zero - _timeProvider.ForcedOffset);
            }
            return null;
        }


        public void QueueEventNotifications(EventEntry entity)
        {
            DequeueEventNotifications(entity);

            var notifyXMinutesAhead = new Action<int>(notifyMinutesAhead =>
            {
                try
                {
                    var deliveryDate =
                        GetSanitizedDeliveryDate(entity.EventDateTimeUtc.AddMinutes(0 - notifyMinutesAhead));
                    if (!deliveryDate.HasValue) return;

                    var parts = SplitGuid(entity.Id);

                    var toast = new ScheduledToastNotification(
                        CreateToast($"{entity.Title} {entity.SubTitle}",
                            $"Event starts in {notifyMinutesAhead} minutes",
                            $"toast:eventDetail:{entity.Id}"),
                        deliveryDate.Value)
                    {
                        Id = parts.Item1,
                        Tag = parts.Item2,
                        Group = $"e:{notifyMinutesAhead}min",
                    };

                    _toastNotificationManager.AddToSchedule(toast);
                }
                catch (Exception)
                {

                }
            });

            notifyXMinutesAhead(60);
            notifyXMinutesAhead(30);
            notifyXMinutesAhead(15);
        }


        public void DequeueEventNotifications(EventEntry entity)
        {
            var parts = SplitGuid(entity.Id);

            var toasts = _toastNotificationManager
                .GetScheduledToastNotifications()
                .Where(a => a.Group.StartsWith("e:") && a.Id == parts.Item1 && a.Tag == parts.Item2);

            foreach (var toast in toasts)
            {
                _toastNotificationManager.RemoveFromSchedule(toast);
            }
        }

        private static Tuple<string, string> SplitGuid(Guid id)
        {
            var bytes = id.ToByteArray();
            return new Tuple<string, string>(Convert.ToBase64String(bytes, 8, 8), Convert.ToBase64String(bytes, 0, 8));
        }

        private static Guid AssembleGuid(string a, string b)
        {
            var bytes = new byte[16];

            Array.Copy(Convert.FromBase64String(a), 0, bytes, 8, 8);
            Array.Copy(Convert.FromBase64String(b), 0, bytes, 0, 8);

            return new Guid(bytes);
        }

        private void RescheduleAllEventNotifications()
        {
            var toasts = _toastNotificationManager
                .GetScheduledToastNotifications()
                .Where(a => a.Group.StartsWith("e:"));

            foreach (var toast in toasts)
            {
                _toastNotificationManager.RemoveFromSchedule(toast);
            }

            foreach (var @event in _dataContext.EventEntries.Where(a => a.AttributesProxy.Extension.IsFavorite))
            {
                QueueEventNotifications(@event);
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
