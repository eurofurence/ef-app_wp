using System;
using System.Diagnostics;
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

            await new MessageDialog(e.Exception.Message).ShowAsync();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (_isInitialized)
            {
                HandleLaunchActivatedEvent(e);
                return;
            }

            var statusBar = StatusBar.GetForCurrentView();
            //statusBar.ForegroundColor = Color.FromArgb(50, 0xff, 0xff, 0xff);
            await statusBar.HideAsync();


            var staticLoadingPage = new StaticLoadingPage();
            Window.Current.Content = staticLoadingPage;
            Window.Current.Activate();

            staticLoadingPage.PageFadeInAsync();


            ThemeManager.SetThemeColor((Color)Resources["EurofurenceThemeColor"]);


            var contextManager = KernelResolver.Current.Get<ContextManager>();
            var navigationMediator = KernelResolver.Current.Get<INavigationMediator>();

            await contextManager.InitializeAsync();

            var layoutPage = new LayoutPage(navigationMediator, _telemetryClientProvider);
            KernelResolver.Current.Bind<ILayoutPage>().ToConstant(layoutPage);

            

            var s = KernelResolver.Current.Get<ToastNotificationService>();


            var startupMode = await GetStartupModeAsync();
            if (startupMode == StartupMode.Close)
            {
                Current.Exit();
                return;
            }


            _rootFrame = layoutPage.RootFrame;


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
                // Removes the turnstile navigation for startup.
                if (_rootFrame.ContentTransitions != null)
                {
                    _transitions = new TransitionCollection();
                    foreach (var c in _rootFrame.ContentTransitions)
                    {
                        _transitions.Add(c);
                    }
                }

                _rootFrame.ContentTransitions = null;
                _rootFrame.Navigated += RootFrame_FirstNavigated;

                if (!await navigationMediator.NavigateAsync(typeof (MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }

                if (startupMode == StartupMode.RunAsFirstStart)
                {
                    await navigationMediator.NavigateAsync(typeof(FirstStartPage));
                }
                else if (HasInternetAccess)
                {
                    var updateTask = KernelResolver.Current.Get<ContextManager>()
                        .Update(doSaveToStoreBeforeUpdate: false);
                    Task.WaitAll(new[] { updateTask }, TimeSpan.FromSeconds(10));
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

        private void HandleLaunchActivatedEvent(LaunchActivatedEventArgs e)
        {
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
            .GetInternetConnectionProfile()
            .GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

        public async Task<StartupMode> GetStartupModeAsync()
        {
            //if (Debugger.IsAttached) await Task.Delay(TimeSpan.FromSeconds(5));

            var applicationSettingsContext = KernelResolver.Current.Get<ApplicationSettingsContext>();
            var appVersionProvider = KernelResolver.Current.Get<IAppVersionProvider>();

            if (applicationSettingsContext.LastPackageVersionRunning != appVersionProvider.GetAppVersion())
            {
                switch (applicationSettingsContext.LastPackageVersionRunning)
                {
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