using Eurofurence.Companion.Common;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;

namespace Eurofurence.Companion.Services
{
    [IocBeacon(TargetType = typeof(PushService))]
    public class PushService
    {
        private const string ChannelUriKey = "ChannelUri";
        private const string ChannelUriDefault = null;

        private PushNotificationChannel _channel;

        private string _channelUri;
        private EurofurenceWebApiClient _apiClient;
        private readonly ApplicationSettingsContext _applicationSettingsContext;
        private readonly ContextManager _contextManager;
        private readonly CoreDispatcher _dispatcher;
        private readonly IAppVersionProvider _appVersionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Services.PushService"/> class.
        /// </summary>
        /// <param name="applicationSettingsContext"></param>
        /// <param name="contextManager"></param>
        /// <param name="dispatcher"></param>
        public PushService(
            ApplicationSettingsContext applicationSettingsContext, 
            ContextManager contextManager, 
            CoreDispatcher dispatcher,
            IAppVersionProvider appVersionProvider)
        {
            _channelUri = LocalSettingsLoad(ApplicationData.Current.LocalSettings, ChannelUriKey, ChannelUriDefault);
            _apiClient = new EurofurenceWebApiClient(Consts.WEB_API_ENDPOINT_URL);
            _applicationSettingsContext = applicationSettingsContext;
            _contextManager = contextManager;
            _dispatcher = dispatcher;
            _appVersionProvider = appVersionProvider;
        }

        public string ChannelUri
        {
            get { return _channelUri; }
            private set
            {
                if (_channelUri != value)
                {
                    this._channelUri = value;
                    LocalSettingsStore(ApplicationData.Current.LocalSettings, ChannelUriKey, value);
                }
            }
        }

        public async Task<string> UpdateChannelUri()
        {
            var retries = 3;
            var difference = 1; // In seconds

            var currentRetry = 0;

            do
            {
                try
                {
                    _channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                    _channel.PushNotificationReceived += OnPushNotificationReceived;
                    if (!_channel.Uri.Equals(ChannelUri))
                    {
                        ChannelUri = _channel.Uri;
                        this.RaiseChannelUriUpdated();


                        var topics = new List<string>() { "Debug" };
                        topics.Add($"Version-{_appVersionProvider.GetAppVersion()}");
                        if (Debugger.IsAttached) topics.Add("Emulator");

                        await _apiClient.PostAsync<object, object>("PushNotifications/WnsChannelRegistration", new
                        {
                            DeviceId = _applicationSettingsContext.UniqueRandomUserId,
                            ChannelUri = _channel.Uri,
                            Topics = topics
                        });

                    }

                    return _channel.Uri;
                }
                catch (Exception ex)
                {
                    // Could not create a channel
                }

                await Task.Delay(TimeSpan.FromSeconds(difference));

            } while (currentRetry++ < retries);

            return null;
        }

        private void OnPushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            switch (args.NotificationType)
            {
                case PushNotificationType.Badge:
                    this.OnBadgeNotificationReceived(args.BadgeNotification.Content.GetXml());
                    break;

                case PushNotificationType.Tile:
                    this.OnTileNotificationReceived(args.TileNotification.Content.GetXml());
                    break;

                case PushNotificationType.Toast:
                    this.OnToastNotificationReceived(args.ToastNotification.Content.GetXml());
                    break;

                case PushNotificationType.Raw:

                    if (args.RawNotification.Content == "update" && _contextManager.UpdateStatus == TaskStatus.RanToCompletion)
                    {
                        _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => _contextManager.Update());
                    }
                    break;
            }

            args.Cancel = true;
        }

        private void OnBadgeNotificationReceived(string notificationContent)
        {
            // Code when a badge notification is received when app is running
        }

        private void OnTileNotificationReceived(string notificationContent)
        {
            // Code when a tile notification is received when app is running
        }

        private void OnToastNotificationReceived(string notificationContent)
        {
            // Code when a toast notification is received when app is running

            // Show a toast notification programatically

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(notificationContent);
            var toastNotification = new ToastNotification(xmlDocument);

            //toastNotification.SuppressPopup = true;
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        private void OnRawNotificationReceived(string notificationContent)
        {
            // Code when a raw notification is received when app is running
        }

        public event EventHandler<EventArgs> ChannelUriUpdated;
        private void RaiseChannelUriUpdated()
        {
            if (ChannelUriUpdated != null)
            {
                ChannelUriUpdated(this, new EventArgs());
            }
        }

        public static T LocalSettingsLoad<T>(ApplicationDataContainer settings, string key, T defaultValue)
        {
            T value;

            if (settings.Values.ContainsKey(key))
            {
                value = (T)settings.Values[key];
            }
            else
            {
                // Otherwise use the default value.
                value = defaultValue;
            }

            return value;
        }

        public static bool LocalSettingsStore(ApplicationDataContainer settings, string key, object value)
        {
            bool valueChanged = false;

            if (settings.Values.ContainsKey(key))
            {
                // If the key exists
                if (settings.Values[key] != value)
                {
                    // If the value has changed, store the new value
                    settings.Values[key] = value;
                    valueChanged = true;
                }
            }
            else
            {
                // Otherwise create the key
                settings.Values.Add(key, value);
                valueChanged = true;
            }

            return valueChanged;
        }
    }
}
