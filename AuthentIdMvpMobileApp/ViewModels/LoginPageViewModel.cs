using AuthentIdMvpMobileApp.Interfaces.Repository;
using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.Models;
using AuthentIdMvpMobileApp.Repository;
using AuthentIdMvpMobileApp.Services.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthentIdMvpMobileApp.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        IUserService _userDataService;
        IGenericRepository _genericRepository;
        public Command LoginCommand { get; }
        [ObservableProperty]
        string userName;
        [ObservableProperty]
        string password;
        [ObservableProperty]
        AuthentIdUser user;
        
        public LoginPageViewModel()
        {

            _genericRepository = new GenericRepository();
            _userDataService = new UserDataService(_genericRepository);
            LoginCommand = new Command(async () => await GetUserAsync());
        }

        async Task GetUserAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var currentUser = await _userDataService.LoginUser(UserName, Password);
                if (currentUser.Count == 1)
                {
                    IsBusy = false;
                    Preferences.Set("CurrentUserId", currentUser[0].Id);
                    Preferences.Set("CurrentUserFirstName", currentUser[0].FirstName);
                    UserName = "";
                    Password = "";
                    Application.Current.MainPage = new AppShell();
                    return;
                }
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Error!", "Unable to verify login details", "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to verify login details: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "Ok");
            }
            finally { IsBusy = false; }
        }

    }
}
