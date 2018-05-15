using System;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel;
using Eurofurence.Companion.Views;
using Ninject;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.Services;
using Eurofurence.Companion.ViewModel.Abstractions;
using Eurofurence.Companion.ViewModel.Local;
using Microsoft.HockeyApp;
using System.Threading.Tasks;
using Eurofurence.Companion.Services.Abstractions;
using Windows.ApplicationModel.Background;
using Windows.UI.Core;
using Eurofurence.Companion.DataModel;

namespace Eurofurence.Companion
{
    public sealed partial class App : Application
    {
        public static string DatabaseStoragePath =
            Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite"));

        private bool _isInitialized;
        private Frame _rootFrame;
        private TransitionCollection _transitions;
        private readonly ITelemetryClientProvider _telemetryClientProvider;
        private CoreDispatcher _dispatcher;


        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

            HockeyClient.Current.Configure("790c6dfa20ff4523834501fcea150ec1");

            _telemetryClientProvider = KernelResolver.Current.Get<ITelemetryClientProvider>();
            _telemetryClientProvider.Client.TrackEvent("Application started");

            UnhandledException += App_UnhandledException;
        }


        private async void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _telemetryClientProvider.Client.TrackException(e.Exception);
            _telemetryClientProvider.Client.Flush();

            //await new MessageDialog(e.Exception.Message).ShowAsync();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (_isInitialized)
            {
                await KernelResolver.Current.Get<AuthenticationService>().EnforceTokenExpirationAsync();
                await KernelResolver.Current.Get<PrivateMessageService>().QueryPrivateMessagesAsync();

                HandleLaunchActivatedEvent(e);
                return;
            }


            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            BindableBase.InjectedCoreDispatcher = _dispatcher;
            KernelResolver.Current.Bind<CoreDispatcher>().ToMethod((c) => _dispatcher);


            var statusBar = StatusBar.GetForCurrentView();
            await statusBar.HideAsync();

            await RegisterBackgroundTasksAsync();
            


            var staticLoadingPage = new StaticLoadingPage();
            Window.Current.Content = staticLoadingPage;
            Window.Current.Activate();

            staticLoadingPage.PageFadeInAsync();

            ThemeManager.SetThemeColor((Color)Resources["EurofurenceThemeColor"]);

            var contextManager = KernelResolver.Current.Get<ContextManager>();
            var navigationMediator = KernelResolver.Current.Get<INavigationMediator>();
            var pushService = KernelResolver.Current.Get<PushService>();

            await contextManager.InitializeAsync();

            var layoutPage = new LayoutPage(navigationMediator, _telemetryClientProvider);
            KernelResolver.Current.Bind<ILayoutPage>().ToConstant(layoutPage);

            KernelResolver.Current.Get<ToastNotificationService>();
            KernelResolver.Current.Get<IBackgroundUpdateService>().SetInterval(TimeSpan.FromMinutes(1));

            var startupMode = await GetStartupModeAsync();
            if (startupMode == StartupMode.Close)
            {
                Current.Exit();
                return;
            }

            await KernelResolver.Current.Get<AuthenticationService>().EnforceTokenExpirationAsync();
            await pushService.TryUpdateChannelUri();


            _rootFrame = layoutPage.RootFrame;
            _rootFrame.ContentTransitions = null;


            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active.
            if (_rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page.
                _rootFrame = new Frame();

                // Associate the frame with a SuspensionManager key.
                SuspensionManager.RegisterFrame(_rootFrame, "AppFrame");

                // TODO: Change this value to a cache size that is appropriate for your application.
                _rootFrame.CacheSize = 1;

                // Set the default language
                _rootFrame.Language = ApplicationLanguages.Languages[0];

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate.
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        // Something went wrong restoring state.
                        // Assume there is no state and continue.
                    }
                }

                // Place the frame in the current Window.
                Window.Current.Content = _rootFrame;
            }

            if (_rootFrame.Content == null)
            {
                if (!await navigationMediator.NavigateAsync(typeof(MainPage), e.Arguments, useTransition: false))
                {
                    throw new Exception("Failed to create initial page");
                }

                if (startupMode == StartupMode.RunAsFirstStart)
                {
                    await navigationMediator.NavigateAsync(typeof(FirstStartPage), null, useTransition: false);
                    //await navigationMediator.NavigateAsync(typeof(LoadingPage), null, useTransition: false);
                }
                else if (HasInternetAccess)
                {
                    var updateTask = KernelResolver.Current.Get<ContextManager>()
                        .Update(doSaveToStoreBeforeUpdate: false);

                    var queryPmTask = KernelResolver.Current.Get<PrivateMessageService>()
                        .QueryPrivateMessagesAsync();

                    await Task.Run(() => Task.WaitAll(new[] { updateTask, queryPmTask }, TimeSpan.FromSeconds(10)));

                    KernelResolver.Current.Get<IBackgroundUpdateService>().Start();
                }
            }


            // Ensure the current window is active.
            _isInitialized = true;

            HandleLaunchActivatedEvent(e);

            await staticLoadingPage.PageFadeOutAsync();
            Window.Current.Content = layoutPage;

            layoutPage.Reveal();

            await HockeyClient.Current.SendCrashesAsync();
        }

        private async Task RegisterBackgroundTasksAsync()
        {
            await BackgroundExecutionManager.RequestAccessAsync();

            // Unregister all tasks - we'll create a new one.
            foreach (var task in BackgroundTaskRegistration.AllTasks.ToList())
            {
                task.Value.Unregister(true);
            }

            var taskBuilder = new BackgroundTaskBuilder();
            taskBuilder.Name = "PushHandler";
            var trigger = new PushNotificationTrigger();
            taskBuilder.SetTrigger(trigger);
            taskBuilder.TaskEntryPoint = typeof(PushHandlerBackgroundTask.TaskImplementation).FullName;

            BackgroundTaskRegistration registration = taskBuilder.Register();
        }

        private void HandleLaunchActivatedEvent(LaunchActivatedEventArgs e)
        {
            // App tries to sync every time on startup - if the phone has internet connectivity, the data context will be up to date
            // by the time this gets executed.

            if (e.Arguments.StartsWith("toast:"))
            {
                var args = e.Arguments.Split(':');
                if (args[1] == "eventDetail")
                {
                    var eventId = Guid.Parse(args[2]);
                    var evm = KernelResolver.Current.Get<IEventsViewModelContext>();
                    var x = evm.EventEntries.Single(a => a.Entity.Id == eventId);
                    KernelResolver.Current.Get<NavigationViewModel>().NavigateToEventDetailPage.Execute(x);
                }
                else if (args[1] == "announcementDetail")
                {
                    var announcementId = Guid.Parse(args[2]);

                    var item = KernelResolver.Current.Get<ActiveAnnouncementsViewModel>()
                        .ActiveAnnouncements.SingleOrDefault(a => a.Id == announcementId);

                    if (item == null) return;
                    KernelResolver.Current.Get<NavigationViewModel>().NavigateToAnnouncementDetailPage.Execute(item);
                }
            }

        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            if (rootFrame != null)
            {
                rootFrame.ContentTransitions = _transitions ?? new TransitionCollection {new NavigationThemeTransition()};
                rootFrame.Navigated -= RootFrame_FirstNavigated;
            }
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            await KernelResolver.Current.Get<IDataContext>().SaveToStoreAsync();

            _telemetryClientProvider.Client.TrackEvent("Application suspended");
            _telemetryClientProvider.Client.Flush();

            await SuspensionManager.SaveAsync();
            
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            HockeyClient.Current.HandleReactivationOfFeedbackFilePicker(args);

            base.OnActivated(args);
        }


        private bool HasInternetAccess => NetworkInformation
            .GetInternetConnectionProfile()?
            .GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

        public async Task<StartupMode> GetStartupModeAsync()
        {
            var applicationSettingsContext = KernelResolver.Current.Get<ApplicationSettingsContext>();
            var appVersionProvider = KernelResolver.Current.Get<IAppVersionProvider>();

            if (applicationSettingsContext.LastPackageVersionRunning != appVersionProvider.GetAppVersion())
            {
                switch (applicationSettingsContext.LastPackageVersionRunning)
                {
                    // Accepted previous versions that won't require a reset
                    case "2.0.0.1":
                        break;
                    default:
                        var contextManager = KernelResolver.Current.Get<ContextManager>();
                        await contextManager.ClearAll();
                        break;
                }

                applicationSettingsContext.LastPackageVersionRunning = appVersionProvider.GetAppVersion();
            }

            if (applicationSettingsContext.LastServerQueryDateTimeUtc.HasValue)
            {
                return StartupMode.RunNormally;
            }

            return StartupMode.RunAsFirstStart;
        }
    }

    public enum StartupMode
    {
        RunNormally,
        RunAsFirstStart,
        Close
    }
}