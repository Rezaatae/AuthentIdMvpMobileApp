using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.Interfaces.Services
{
    public interface INfcService
    {
        /// <summary>
        /// Configuration for NFC service 
        /// </summary>
        public void ConfigureNfcAdapter()
        {

        }

        /// <summary>
        /// Enable NFC search
        /// </summary>
        public void EnableForegroundDispatch()
        {

        }

        /// <summary>
        /// Disable NFC search
        /// </summary>
        public void DisableForegroundDispatch()
        {

        }

        /// <summary>
        /// Unconfigure NFC services
        /// </summary>
        public void UnconfigureNfcAdapter()
        {

        }


        /// <summary>
        /// Get NFC tag data
        /// </summary>
        public Task<string> GetTagData();

        /// <summary>
        /// Open NFC settings, This is only for Android.
        /// </summary>
        /// <returns></returns>
        public Task<bool> OpenNFCSettingsAsync() => Task.FromResult(true);
    }
}
