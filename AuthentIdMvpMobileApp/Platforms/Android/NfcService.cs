using Android.App;
using Android.Content;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.OS;
using AuthentIdMvpMobileApp.Enums;
using AuthentIdMvpMobileApp.Interfaces.Services;
using Application = Microsoft.Maui.Controls.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java.Security;

namespace AuthentIdMvpMobileApp.Platforms.Android
{
    public class NfcService : INfcService
    {
        private readonly MainActivity mainActivity = (MainActivity)Platform.CurrentActivity;
        private Lazy<NfcAdapter> lazynfcAdaptor = new Lazy<NfcAdapter>(() => NfcAdapter.GetDefaultAdapter(Platform.CurrentActivity));
        private NfcAdapter NfcAdapter => lazynfcAdaptor.Value;
        private PendingIntent pendingIntent;
        private IntentFilter[] writeTagFilters;
        private string[][] techList;
        private ReaderCallback readerCallback;
        private NfcStatus NfcStatus => NfcAdapter == null ?
            NfcStatus.Unavailable : NfcAdapter.IsEnabled ?
            NfcStatus.Enabled : NfcStatus.Disabled;
        
        public static Tag DetectedTag { get; set; }

        public NfcService()
        {
            Platform.ActivityStateChanged += Platform_ActivityStateChanged;
        }

        private void Platform_ActivityStateChanged(object sender, ActivityStateChangedEventArgs e)
        {
            switch (e.State)
            {
                case ActivityState.Resumed:
                    EnableForegroundDispatch();
                    break;

                case ActivityState.Paused:
                    DisableForegroundDispatch();
                    break;
            }
        }

        public void ConfigureNfcAdaptor()
        {
            IntentFilter tagDiscovered = new IntentFilter(NfcAdapter.ActionTagDiscovered);
            IntentFilter ndefDiscovered = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
            IntentFilter techDiscovered = new IntentFilter(NfcAdapter.ActionTechDiscovered);
            tagDiscovered.AddCategory(Intent.CategoryDefault);
            ndefDiscovered.AddCategory(Intent.CategoryDefault);
            techDiscovered.AddCategory(Intent.CategoryDefault);

            var intent = new Intent(mainActivity, mainActivity.Class).AddFlags(ActivityFlags.SingleTop);
            pendingIntent = PendingIntent.GetActivity(mainActivity, 0, intent, 0);

            techList = new string[][]
            {
                new string[] { nameof(NfcA) },
                new string[] { nameof(NfcB) },
                new string[] { nameof(NfcF) },
                new string[] { nameof(NfcV) },
                new string[] { nameof(IsoDep) },
                new string[] { nameof(NdefFormatable) },
                new string[] { nameof(MifareClassic) },
                new string[] { nameof(MifareUltralight) },
            };
            readerCallback = new ReaderCallback();
            writeTagFilters = new IntentFilter[] { tagDiscovered, ndefDiscovered, techDiscovered };
        }

        public void DisableForegroundDispatch()
        {
            NfcAdapter?.DisableForegroundDispatch(Platform.CurrentActivity);
            NfcAdapter?.DisableReaderMode(Platform.CurrentActivity);
        }

        public void EnableForegroundDispatch()
        {
            //NfcAdapter?.EnableForegroundDispatch(Platform.CurrentActivity, pendingIntent, writeTagFilters, techList); //Foreground dispatch API enabled
            NfcAdapter?.EnableReaderMode(Platform.CurrentActivity, readerCallback, NfcReaderFlags.NfcA, null); //Reader mode API enabled
        }

        public void UnconfigureNfcAdapter()
        {
            Platform.ActivityStateChanged -= Platform_ActivityStateChanged;
        }

        private async Task<Tag> GetDetectedTag()
        {
            mainActivity.NfcTag = new TaskCompletionSource<Tag>();
            readerCallback.NFCTag = new TaskCompletionSource<Tag>();
            var tagDetectionTask = await Task.WhenAny(mainActivity.NfcTag.Task, readerCallback.NFCTag.Task);//.TimeoutAfter(TimeSpan.FromSeconds(60));
            return await tagDetectionTask;
        }
        public async Task<bool> OpenNFCSettingsAsync(byte[] bytes)
        {
            if (NfcStatus == NfcStatus.Unavailable)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "NFC is not supported", "Ok");
                return false;
            }

            if (NfcStatus == NfcStatus.Disabled)
            {
                await Application.Current.MainPage.DisplayAlert("Disabled", "NFC is disabled. If you wish to continuew, please enable NFC from settings.", "Ok");
            }

            return true;
        }

        public async Task<string> GetTagData()
        {
            Tag tag = await GetDetectedTag();
            return tag.ToString();
        }
    }
}
