using AuthentIdMvpMobileApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.ViewModels
{
    [QueryProperty(nameof(Scan), nameof(Scan))]
    [QueryProperty(nameof(Agent), nameof(Agent))]
    public partial class ScanDetailPageViewModel : BaseViewModel
    {
        public Command OpenMapCommand { get; }
        IMap map;
        [ObservableProperty]
        AuthentIdScan scan;
        [ObservableProperty]
        AuthentIdAgent agent;
        public ScanDetailPageViewModel(IMap map)
        {
            this.map = map;
            OpenMapCommand = new Command(async () => await OpenMap());
        }


        async Task OpenMap()
        {
            try
            {
                await map.OpenAsync((double)Scan.ScanLatitude, (double)Scan.ScanLongitude, new MapLaunchOptions
                {
                    Name = Scan.ScanDate.ToString("yyyyMMdd"),
                    NavigationMode = NavigationMode.None
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to launch map: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "Ok");
            }
        }
    }
}
