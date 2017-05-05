using Eurofurence.Companion.Common;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZXing;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Eurofurence.Companion.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QrCodeScannerPage : Page
    {
        private MediaCapture _mediaCapture;
        private byte[] imageBuffer;
        public int exit = 0;
        private MessageDialog dialog;
        private ApplicationView currentView = ApplicationView.GetForCurrentView();

        public QrCodeScannerPage()
        {
            InitializeComponent();

            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += NavigationHelper_LoadState;
            NavigationHelper.SaveState += NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper { get;  }



        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }
   
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }
                
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
            exit = 0;
            ScanQrCode();
        }

        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
            exit = 1;
            _mediaCapture?.Dispose();
        }

        private async void ScanQrCode()
        {
            try
            {
                await InitializeQrCode();


                var imgProp = new ImageEncodingProperties
                {
                    Subtype = "BMP",
                    Width = 380,
                    Height = 380
                };
                var bcReader = new BarcodeReader();


                while (exit == 0)
                {
                    var stream = new InMemoryRandomAccessStream();
                    await _mediaCapture.CapturePhotoToStreamAsync(imgProp, stream);


                    stream.Seek(0);
                    var wbm = new WriteableBitmap(380, 380);
                    await wbm.SetSourceAsync(stream);


                    
                    var result = bcReader.Decode(wbm);


                    if (result != null)
                    {
                        var torch = _mediaCapture.VideoDeviceController.TorchControl;
                        if (torch.Supported) torch.Enabled = false;
                        await _mediaCapture.StopPreviewAsync();
                        var msgbox = new MessageDialog(result.Text);
                        await msgbox.ShowAsync();
                    }
                }
            }
            catch { }
        }

        private async Task InitializeQrCode()
        {
            string error = null;
            try
            {
                //if (_mediaCapture == null)  
                //{  
                // Find all available webcams  
                DeviceInformationCollection webcamList = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);


                // Get the proper webcam (default one)  
                DeviceInformation backWebcam = (from webcam in webcamList where webcam.IsEnabled select webcam).FirstOrDefault();


                // Initializing MediaCapture  


                _mediaCapture = new MediaCapture();
                await _mediaCapture.InitializeAsync(new MediaCaptureInitializationSettings
                {
                    VideoDeviceId = backWebcam.Id,
                    AudioDeviceId = "",
                    StreamingCaptureMode = StreamingCaptureMode.Video,
                    PhotoCaptureSource = PhotoCaptureSource.VideoPreview
                });


                // Adjust camera rotation for Phone  
                _mediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
                _mediaCapture.SetRecordRotation(VideoRotation.Clockwise90Degrees);


                // Set the source of CaptureElement to MediaCapture  
                captureElement.Source = _mediaCapture;
                await _mediaCapture.StartPreviewAsync();


                //_mediaCapture.FocusChanged += _mediaCapture_FocusChanged;


                // Seetting Focus & Flash(if Needed)  


                var torch = _mediaCapture.VideoDeviceController.TorchControl;
                if (torch.Supported) torch.Enabled = true;


                await _mediaCapture.VideoDeviceController.FocusControl.UnlockAsync();
                var focusSettings = new FocusSettings();
                focusSettings.AutoFocusRange = AutoFocusRange.FullRange;
                focusSettings.Mode = FocusMode.Continuous;
                focusSettings.WaitForFocus = true;
                focusSettings.DisableDriverFallback = false;
                _mediaCapture.VideoDeviceController.FocusControl.Configure(focusSettings);
                await _mediaCapture.VideoDeviceController.FocusControl.FocusAsync();


                //}  
            }
            catch (Exception ex)
            {
                dialog = new MessageDialog("Error: " + ex.Message);
                dialog.ShowAsync();
            }
        }

        private async Task<byte[]> ConvertWriteableBitmapToByteArray(WriteableBitmap bitmap)
        {
            byte[] pixels;
            using (Stream stream = bitmap.PixelBuffer.AsStream())
            {
                pixels = new byte[(uint)stream.Length];
                await stream.ReadAsync(pixels, 0, pixels.Length);
            }

            return pixels;
        }
    }
}
