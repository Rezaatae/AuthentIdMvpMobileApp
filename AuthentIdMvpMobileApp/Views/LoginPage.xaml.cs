using AuthentIdMvpMobileApp.Interfaces.Repository;
using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.Repository;
using AuthentIdMvpMobileApp.Services.Data;
using AuthentIdMvpMobileApp.ViewModels;

namespace AuthentIdMvpMobileApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginPageViewModel viewModel;
    public LoginPage()
	{
		InitializeComponent();
		BindingContext = new LoginPageViewModel();
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Preferences.Clear();
        Console.WriteLine("reztest navigated to login page");
    }
}