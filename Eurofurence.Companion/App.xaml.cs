using Eurofurence.Companion.Common;
using Eurofurence.Companion.DataStore;
using Eurofurence.Companion.DependencyResolution;
using Eurofurence.Companion.ViewModel;
using Eurofurence.Companion.Views;
using HockeyApp;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Hub Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Eurofurence.Companion
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        public static string DatabaseStoragePath = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite"));

        private TransitionCollection transitions;
        private Frame RootFrame;
        private bool _isInitialized = false;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;

            HockeyClient.Current.Configure("790c6dfa20ff4523834501fcea150ec1");
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (_isInitialized) return;

            var contextManager = KernelResolver.Current.Get<ContextManager>();
            await contextManager.InitializeAsync();


#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            //RootFrame = Window.Current.Content as Frame;

            var layoutPage = new LayoutPage();
            KernelResolver.Current.Bind<ILayoutPage>().ToConstant(layoutPage);
            

            Window.Current.Content = layoutPage;


            RootFrame = (Window.Current.Content as LayoutPage)?.RootFrame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active.
            if (RootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page.
                RootFrame = new Frame();

                // Associate the frame with a SuspensionManager key.
                SuspensionManager.RegisterFrame(RootFrame, "AppFrame");

                // TODO: Change this value to a cache size that is appropriate for your application.
                RootFrame.CacheSize = 1;

                // Set the default language
                RootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

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
                Window.Current.Content = RootFrame;
            }

            if (RootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (RootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in RootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                RootFrame.ContentTransitions = null;
                RootFrame.Navigated += this.RootFrame_FirstNavigated;
                
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter.
                if (!layoutPage.Navigate(typeof(Views.MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }



            //KernelResolver.Current.Bind<INavigate>().ToConstant(RootFrame);
            //KernelResolver.Current.Get<ViewModelLocator>().NavigationViewModel.NavigateToEventDetailPage.Execute(
            //    KernelResolver.Current.Get<ViewModelLocator>().EventsViewModel.DataContext.EventEntries.FirstOrDefault()
            //    );
            //KernelResolver.Current.Get<ViewModelLocator>().NavigationViewModel.NavigateToEventsByDayPage.Execute(
            //     KernelResolver.Current.Get<ViewModelLocator>().EventsViewModel.DataContext.EventConferenceDays.FirstOrDefault()
            //     );

            var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            statusBar.ForegroundColor = Windows.UI.Color.FromArgb(50, 0xff, 0xff, 0xff);


            // Ensure the current window is active.
            Window.Current.Activate();
            _isInitialized = true;

            await HockeyClient.Current.SendCrashesAsync();
            #if WINDOWS_PHONE_APP
                        await HockeyClient.Current.CheckForAppUpdateAsync();
            #endif
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            HockeyClient.Current.HandleReactivationOfFeedbackFilePicker(args);
            base.OnActivated(args);
        }
    }
}
