using Eurofurence.Companion.Common;
using Eurofurence.Companion.DependencyResolution;
using Ninject;
using System;
using System.Linq;
using Windows.Networking.Connectivity;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Eurofurence.Companion.Common.Abstractions;
using Eurofurence.Companion.DataStore.Abstractions;
using Eurofurence.Companion.ViewModel;
using Eurofurence.Companion.ViewModel.Local;


namespace Eurofurence.Companion.Views
{
    public sealed partial class DebugPage : Page
    {
        private ITimeProvider _timeProvider;

        public DebugPage()
        {
            InitializeComponent();

            _timeProvider = KernelResolver.Current.Get<ITimeProvider>();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;

            this.Loaded += DebugPage_Loaded;


            _tpOffsetDatePicker.Date = _timeProvider.CurrentDateTimeUtc.Date;
            _tpOffsetTimePicker.Time = _timeProvider.CurrentDateTimeUtc.TimeOfDay;
        }



        private async void DebugPage_Loaded(object sender, RoutedEventArgs e)
        {
            var log = new Action<string>((s) => E_TextBlock_Info.Text += s + Environment.NewLine);

            var storageSizes = (await KernelResolver.Current.Get<IDataStore>().GetStorageFileSizesAsync());
            log($"Storage:\n  {storageSizes.Sum(a => (decimal)a.Value)} bytes in {storageSizes.Count} files.");
            log($"Memory:\n  {MemoryManager.AppMemoryUsage}/{MemoryManager.AppMemoryUsageLimit} ({MemoryManager.AppMemoryUsageLevel})");


            var networkProfile = NetworkInformation.GetInternetConnectionProfile();
            log($"Network:\n  Connectivity: {networkProfile.GetNetworkConnectivityLevel()}");
        }

        public NavigationHelper NavigationHelper { get; }

        public ObservableDictionary DefaultViewModel { get; } = new ObservableDictionary();

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void _btnSetTimeProviderOffset_Click(object sender, RoutedEventArgs e)
        {
            _timeProvider.ForcedOffset =
                _tpOffsetDatePicker.Date + _tpOffsetTimePicker.Time - DateTime.UtcNow;
        }

        private void E_Button_ToggleFrameRateCounter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Application.Current.DebugSettings.EnableFrameRateCounter = !Application.Current.DebugSettings.EnableFrameRateCounter;
        }

        private void E_Button_ToastTest_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);


            var dueTime = DateTime.Now.AddSeconds(5);
                
            var strings = toastXml.GetElementsByTagName("text");
            strings[0].AppendChild(toastXml.CreateTextNode("This is a scheduled toast notification"));
            strings[1].AppendChild(toastXml.CreateTextNode("Received: " + dueTime.ToString()));

            // Create the toast notification object.

            var toast = new ScheduledToastNotification(toastXml, dueTime)
            {
                Id = Math.Floor(new Random().NextDouble()*1000000).ToString()
            };



            // Add to the schedule.
            var foo = ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications();

            if (foo.Count > 0)
            {
                
            }

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);

            var foo2 = ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications();
        }

        private void E_Button_ToggleDebugMode_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var m = KernelResolver.Current.Get<DebugViewModel>();
            m.IsDebugMode = !m.IsDebugMode;
        }

        private void E_Button_QRCode_Tapped(object sender, TappedRoutedEventArgs e)
        {
            KernelResolver.Current.Get<INavigationMediator>().NavigateAsync(typeof(QrCodeScannerPage));
        }
    }
}
