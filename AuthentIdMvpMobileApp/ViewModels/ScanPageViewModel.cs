using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.Models;
using AuthentIdMvpMobileApp.Services.Data;
using AuthentIdMvpMobileApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.ViewModels
{
    public partial class ScanPageViewModel : BaseViewModel
    {
        public Command ScanCommand { get; }
        ScanDataService _scanDataSercvice;
        AgentDataService _agentDataService;
        [ObservableProperty]
        AuthentIdScan newScan;

        public const string ALERT_TITLE = "NFC";
        public const string MIME_TYPE = "application/com.companyname.nfcsample";

        NFCNdefTypeFormat _type;
        bool _makeReadOnly = false;
        bool _eventsAlreadySubscribed = false;
        bool _isDeviceiOS = false;
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public ScanPageViewModel(IScanService scanService, IAgentService agentService)
        {
            newScan = new AuthentIdScan();
            _agentDataService = (AgentDataService)agentService;
            _scanDataSercvice = (ScanDataService)scanService;
            ScanCommand = new Command(async () => await ScanAsync());

        }

        public bool DeviceIsListening
        {
            get => _deviceIsListening;
            set
            {
                _deviceIsListening = value;
                OnPropertyChanged(nameof(DeviceIsListening));
            }
        }
        private bool _deviceIsListening;

        private bool _nfcIsEnabled;
        public bool NfcIsEnabled
        {
            get => _nfcIsEnabled;
            set
            {
                _nfcIsEnabled = value;
                OnPropertyChanged(nameof(NfcIsEnabled));
                OnPropertyChanged(nameof(NfcIsDisabled));
            }
        }
        public bool NfcIsDisabled => !NfcIsEnabled;

        async Task ScanAsync()
        {
            if (IsBusy) return; 

            try
            {
                IsBusy = true;
                Console.WriteLine("reztest isbusy try");


                // In order to support Mifare Classic 1K tags (read/write), you must set legacy mode to true.
                CrossNFC.Legacy = false;

                if (CrossNFC.IsSupported)
                {
                    if (!CrossNFC.Current.IsAvailable)
                        await ShowAlert("NFC is not available");

                    NfcIsEnabled = CrossNFC.Current.IsEnabled;
                    if (!NfcIsEnabled)
                        await ShowAlert("NFC is disabled");

                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                        _isDeviceiOS = true;

                    Button_Clicked_StartListening();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable scan tag: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "Ok");
            }
            finally
            {
                Console.WriteLine("reztest isbusy finally");
            }
        }


        /// <summary>
        /// Auto Start Listening
        /// </summary>
        /// <returns></returns>
        async Task AutoStartAsync()
        {
            // Some delay to prevent Java.Lang.IllegalStateException "Foreground dispatch can only be enabled when your activity is resumed" on Android
            await Task.Delay(500);
            await StartListeningIfNotiOS();
        }

        /// <summary>
        /// Subscribe to the NFC events
        /// </summary>
        void SubscribeEvents()
        {
            if (_eventsAlreadySubscribed)
                UnsubscribeEvents();

            _eventsAlreadySubscribed = true;

            CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;

            if (_isDeviceiOS)
                CrossNFC.Current.OniOSReadingSessionCancelled += Current_OniOSReadingSessionCancelled;
        }

        /// <summary>
        /// Unsubscribe from the NFC events
        /// </summary>
        void UnsubscribeEvents()
        {
            CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;

            if (_isDeviceiOS)
                CrossNFC.Current.OniOSReadingSessionCancelled -= Current_OniOSReadingSessionCancelled;

            _eventsAlreadySubscribed = false;
        }

        /// <summary>
        /// Event raised when Listener Status has changed
        /// </summary>
        /// <param name="isListening"></param>
        void Current_OnTagListeningStatusChanged(bool isListening) => DeviceIsListening = isListening;

        /// <summary>
        /// Event raised when NFC Status has changed
        /// </summary>
        /// <param name="isEnabled">NFC status</param>
        async void Current_OnNfcStatusChanged(bool isEnabled)
        {
            NfcIsEnabled = isEnabled;
            await ShowAlert($"NFC has been {(isEnabled ? "enabled" : "disabled")}");
        }


        /// <summary>
        /// Event raised when a NDEF message is received
        /// </summary>
        /// <param name="tagInfo">Received <see cref="ITagInfo"/></param>
        async void Current_OnMessageReceived(ITagInfo tagInfo)
        {
            if (tagInfo == null)
            {
                await ShowAlert("No tag found");
                return;
            }

            // Customized serial number
            var identifier = tagInfo.Identifier;
            var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");
            var title = !string.IsNullOrWhiteSpace(serialNumber) ? $"Tag [{serialNumber}]" : "Tag Info";

            if (!tagInfo.IsSupported)
            {
                await ShowAlert("Unsupported tag (app)", title);
            }
            else if (tagInfo.IsEmpty)
            {
                await ShowAlert("Empty tag", title);
            }
            else
            {
                // check datatype
                // check agent exists
                var scannedAgentId = Int32.Parse(tagInfo.Records[0].Message);
                var agentExists = await CheckAgentAgainstDatabaseAsync(scannedAgentId);
                if (agentExists)
                {
                    //await Application.Current.MainPage.DisplayAlert("Agent Verified!", $"Agent is valid!", "Ok");
                    CrossNFC.Current.StopListening();
                    var agent = await _agentDataService.GetAgent(scannedAgentId);
                    await AddScanFromNFCRecord(agent);
                    return;
                }
                IsBusy = false;
                Console.WriteLine("reztest isbusy Cannot find agent details");
                await Application.Current.MainPage.DisplayAlert("Warning", $"Cannot find agent details", "Ok");
                CrossNFC.Current.StopListening();
                return;
            }
        }

        async Task<bool> CheckAgentAgainstDatabaseAsync(int agentId)
        {
            var agent = await _agentDataService.GetAgent(agentId);
            bool agentExists = agent.Id != 0;
            return agentExists;
        }

        /// <summary>
        /// Event raised when user cancelled NFC session on iOS 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Current_OniOSReadingSessionCancelled(object sender, EventArgs e) => Debug("iOS NFC Session has been cancelled");

        /// <summary>
        /// Event raised when a NFC Tag is discovered
        /// </summary>
        /// <param name="tagInfo"><see cref="ITagInfo"/> to be published</param>
        /// <param name="format">Format the tag</param>
        async void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
        {
            if (!CrossNFC.Current.IsWritingTagSupported)
            {
                await ShowAlert("Writing tag is not supported on this device");
                return;
            }

            try
            {
                NFCNdefRecord record = null;
                switch (_type)
                {
                    case NFCNdefTypeFormat.WellKnown:
                        record = new NFCNdefRecord
                        {
                            TypeFormat = NFCNdefTypeFormat.WellKnown,
                            MimeType = MIME_TYPE,
                            Payload = NFCUtils.EncodeToByteArray("Plugin.NFC is awesome!"),
                            LanguageCode = "en"
                        };
                        break;
                    case NFCNdefTypeFormat.Uri:
                        record = new NFCNdefRecord
                        {
                            TypeFormat = NFCNdefTypeFormat.Uri,
                            Payload = NFCUtils.EncodeToByteArray("https://github.com/franckbour/Plugin.NFC")
                        };
                        break;
                    case NFCNdefTypeFormat.Mime:
                        record = new NFCNdefRecord
                        {
                            TypeFormat = NFCNdefTypeFormat.Mime,
                            MimeType = MIME_TYPE,
                            Payload = NFCUtils.EncodeToByteArray("Plugin.NFC is awesome!")
                        };
                        break;
                    default:
                        break;
                }

                if (!format && record == null)
                    throw new Exception("Record can't be null.");

                tagInfo.Records = new[] { record };

                if (format)
                    CrossNFC.Current.ClearMessage(tagInfo);
                else
                {
                    CrossNFC.Current.PublishMessage(tagInfo, _makeReadOnly);
                }
            }
            catch (Exception ex)
            {
                await ShowAlert(ex.Message);
            }
        }

        /// <summary>
        /// Start listening for NFC Tags when "READ TAG" button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void Button_Clicked_StartListening() => await BeginListening();

        /// <summary>
        /// Stop listening for NFC tags
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Button_Clicked_StopListening() => await StopListening();


        /// <summary>
        /// Returns the tag information from NDEF record
        /// </summary>
        /// <param name="record"><see cref="NFCNdefRecord"/></param>
        /// <returns>The tag information</returns>
        string GetMessage(NFCNdefRecord record)
        {
            var message = $"Message: {record.Message}";
            message += System.Environment.NewLine;
            message += $"RawMessage: {Encoding.UTF8.GetString(record.Payload)}";
            message += System.Environment.NewLine;
            message += $"Type: {record.TypeFormat}";

            if (!string.IsNullOrWhiteSpace(record.MimeType))
            {
                message += System.Environment.NewLine;
                message += $"MimeType: {record.MimeType}";
            }

            return message;
        }

        async Task AddScanFromNFCRecord(AuthentIdAgent agent)
        {
            var loc = await GetCurrentLocation();
            var currentUserId = Preferences.Get("CurrentUserId", 0);

            newScan.AgentId = agent.Id;
            newScan.UserId = currentUserId;
            newScan.ScanLatitude = (decimal)loc.Latitude;
            newScan.ScanLongitude = (decimal)loc.Longitude;
            newScan.ScanDate = DateTime.Now;
            await _scanDataSercvice.AddScanAsync(newScan);
            IsBusy = false;
            Console.WriteLine("reztest isbusy navigate to agent details");
            await Shell.Current.GoToAsync($"{nameof(AgentDetailPage)}", new Dictionary<string, object>
            {
                ["Agent"] = agent
            });
            //await Shell.Current.GoToAsync(nameof(StubDetailView));

        }

        /// <summary>
        /// Write a debug message in the debug console
        /// </summary>
        /// <param name="message">The message to be displayed</param>
        void Debug(string message) => System.Diagnostics.Debug.WriteLine(message);

        /// <summary>
        /// Display an alert
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Alert title</param>
        /// <returns>The task to be performed</returns>
        Task ShowAlert(string message, string title = null) => Application.Current.MainPage.DisplayAlert(string.IsNullOrWhiteSpace(title) ? ALERT_TITLE : title, message, "OK");

        /// <summary>
        /// Task to start listening for NFC tags if the user's device platform is not iOS
        /// </summary>
        /// <returns>The task to be performed</returns>
        async Task StartListeningIfNotiOS()
        {
            if (_isDeviceiOS)
            {
                SubscribeEvents();
                return;
            }
            await BeginListening();
        }

        /// <summary>
        /// Task to safely start listening for NFC Tags
        /// </summary>
        /// <returns>The task to be performed</returns>
        async Task BeginListening()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SubscribeEvents();
                    CrossNFC.Current.StartListening();
                });
            }
            catch (Exception ex)
            {
                await ShowAlert(ex.Message);
            }
        }

        /// <summary>
        /// Task to safely stop listening for NFC tags
        /// </summary>
        /// <returns>The task to be performed</returns>
        async Task StopListening()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CrossNFC.Current.StopListening();
                    UnsubscribeEvents();
                });
            }
            catch (Exception ex)
            {
                await ShowAlert(ex.Message);
            }
        }

        public async Task<Location> GetCurrentLocation()
        {
            Location location = new();
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
                await ShowAlert(ex.Message);
                location = null;
            }
            finally
            {
                _isCheckingLocation = false;
            }
            return location;
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
    }
}
