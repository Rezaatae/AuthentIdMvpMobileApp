using AuthentIdMvpMobileApp.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AuthentIdMvpMobileApp.ViewModels
{
    public class ScanViewModel : BaseViewModel
    {
        public Command ScanCommand { get; }
        private readonly INfcService nfcAdapter;
        private string stringData;

        public ScanViewModel(INfcService nfcService)
        {
            this.nfcAdapter = nfcService;
            ScanCommand = new Command(async () => await ExecuteNfcReader());
        }

        private async Task ExecuteNfcReader()
        {
            Console.WriteLine("Reztest scan button hit");
            if (await nfcAdapter.OpenNFCSettingsAsync())
            {
                Console.WriteLine("reztest start reading tag");
                nfcAdapter.ConfigureNfcAdapter();
                nfcAdapter.EnableForegroundDispatch();

                var detTag = await nfcAdapter.GetTagData();
                Console.WriteLine("reztest retruning tag data");
            }

        }
    }
}
