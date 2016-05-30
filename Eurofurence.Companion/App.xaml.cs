using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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
using HockeyApp;
using Ninject;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.DataContracts;
using Windows.UI.Xaml.Media;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataStore.Abstractions;

namespace Eurofurence.Companion
{
    public sealed partial class App : Application
    {
        public static string DatabaseStoragePath =
            Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite"));

        private bool _isInitialized;
        private Frame _rootFrame;
        private TransitionCollection _transitions;
        private ITelemetryClientProvider _telemetryClientProvider;

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
            if (_isInitialized) return;

            ThemeManager.SetThemeColor((Color)Resources["EurofurenceThemeColor"]);


            var contextManager = KernelResolver.Current.Get<ContextManager>();
            var navigationMediator = KernelResolver.Current.Get<INavigationMediator>();
            await contextManager.InitializeAsync();

#if DEBUG
            if (Debugger.IsAttached)
            {
                //this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            var layoutPage = new LayoutPage(navigationMediator, _telemetryClientProvider);
            KernelResolver.Current.Bind<ILayoutPage>().ToConstant(layoutPage);
            Window.Current.Content = layoutPage;
            layoutPage.OnLayoutPageRendered();


            var firstTimeRunResult = await HandleFirstTimeRunAsync();
            if (firstTimeRunResult == FirstTimeRunResult.Close)
            {
                Current.Exit();
                return;
            }

            _rootFrame = (Window.Current.Content as LayoutPage)?.RootFrame;


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

                //// When the navigation stack isn't restored navigate to the first page,
                //// configuring the new page by passing required information as a navigation
                //// parameter.
                if (!(await navigationMediator.NavigateAsync(typeof (MainPage), e.Arguments)))
                {
                    throw new Exception("Failed to create initial page");
                }

                //navigationMediator.Navigate(typeof(WelcomeGuidePage));
                //navigationMediator.Navigate(typeof(LoadingPage));

                if (firstTimeRunResult == FirstTimeRunResult.RunAndSynchronize)
                {
                    await navigationMediator.NavigateAsync(
                        typeof (LoadingPage),
                        new LoadingPage.LoadingPageOptions
                        {
                            MustCompleteSuccessfully = true,
                            AutoNavigateBackOnSuccess = true,
                            AutoStartUpdateOnNavigatedTo = true
                        });
                } 
            }


            var statusBar = StatusBar.GetForCurrentView();
            statusBar.ForegroundColor = Color.FromArgb(50, 0xff, 0xff, 0xff);
            await statusBar.HideAsync();

            // Ensure the current window is active.
            Window.Current.Activate();
            _isInitialized = true;

            await HockeyClient.Current.SendCrashesAsync();
#if WINDOWS_PHONE_APP
            await HockeyClient.Current.CheckForAppUpdateAsync();
#endif
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
            await KernelResolver.Current.Get<IDataContext>().SaveAsync();

            _telemetryClientProvider.Client.TrackEvent("Application suspended");
            _telemetryClientProvider.Client.Flush();

            await SuspensionManager.SaveAsync();

            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            HockeyClient.Current.HandleReactivationOfFeedbackFilePicker(args);
            base.OnActivated(args);
        }

        public async Task<FirstTimeRunResult> HandleFirstTimeRunAsync()
        {
            var context = KernelResolver.Current.Get<ApplicationSettingsContext>();
            if (context.LastServerQueryDateTimeUtc.HasValue)
            {
                return FirstTimeRunResult.RunNormally;
            }

            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            var connectivityLevel = connectionProfile.GetNetworkConnectivityLevel();
            if (connectivityLevel != NetworkConnectivityLevel.InternetAccess)
            {
                var dlg =
                    new MessageDialog(
                        "Before you can use this application, we need to download some data from the Eurofurence servers to your phone.\n\nYour phone indicates that it currently does not have any internet connectivity.\n\nPlease restart the application again when your phone has internet connectivity.\n\nThis is only required once. When the convention data has been synchronized to your phone, you can use the app offline.\n\nThe application will now close.",
                        "Welcome to the Eurofurence app for Windows Phone!");
                {
                }
                ;

                await dlg.ShowAsync();
                return FirstTimeRunResult.Close;
            }


            var dialogResult = FirstTimeRunResult.Close;

            var messageDialog = new MessageDialog(
                "Before you can use this application, we need to download some data from the Eurofurence servers to your phone.\n\nThis will consume a few megabytes of traffic and can take anywhere from a few seconds up to a few minutes, depending on the speed of your connection.\n\nIs it okay to download the data now?\n\nChosing 'no' will close the application at this point.",
                "Welcome!");
            messageDialog.Commands.Add(new UICommand("Yes",
                cmd => { dialogResult = FirstTimeRunResult.RunAndSynchronize; }));
            messageDialog.Commands.Add(new UICommand("No", cmd => { dialogResult = FirstTimeRunResult.Close; }));

            await messageDialog.ShowAsync();

            return dialogResult;
        }
    }

    public enum FirstTimeRunResult
    {
        RunNormally,
        RunAndSynchronize,
        Close
    }
}