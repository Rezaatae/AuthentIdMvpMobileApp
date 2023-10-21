using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.Services.Data;
using AuthentIdMvpMobileApp.ViewModels;
using AuthentIdMvpMobileApp.Views;

namespace AuthentIdMvpMobileApp;

public partial class App : Application
{

    public App()
	{
		InitializeComponent();
		MainPage = new LoginPage();
		//MainPage = new AppShell();
	}
}
