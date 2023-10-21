using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.Models;
using AuthentIdMvpMobileApp.Services.Data;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.ViewModels
{
    public partial class ScanHistoryPageViewModel : BaseViewModel
    {
        public ObservableCollection<AuthentIdScan> Scans { get; } = new();
        public Command GetScansCommand { get; }
        UserDataService _userService;

        public ScanHistoryPageViewModel(IUserService userService)
        {
            _userService = (UserDataService)userService;
            GetScansCommand = new Command(async () => await GetUserScansAsync());
        }

        async Task GetUserScansAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var currentUserId = Preferences.Get("CurrentUserId", 0);
                var userScans = await _userService.GetUserScans(currentUserId);

                if (Scans.Count != 0)
                {
                    Scans.Clear();
                }
                foreach(var scan in userScans)
                {
                    Scans.Add(scan);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get scans: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "Ok");
            }
            finally { IsBusy = false; }
        }
    }
}
