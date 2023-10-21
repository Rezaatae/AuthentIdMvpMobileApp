using AuthentIdMvpMobileApp.Interfaces.Repository;
using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.Repository;
using AuthentIdMvpMobileApp.Services.Data;
using AuthentIdMvpMobileApp.ViewModels;
using AuthentIdMvpMobileApp.Views;
using Microsoft.Extensions.Logging;

namespace AuthentIdMvpMobileApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<IGenericRepository, GenericRepository>();
		builder.Services.AddSingleton<IAgentService, AgentDataService>();
		builder.Services.AddSingleton<IScanService, ScanDataService>();
		builder.Services.AddSingleton<IUserService, UserDataService>();
		builder.Services.AddSingleton<IMap>(Map.Default);

		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<LoginPageViewModel>();
		builder.Services.AddTransient<HomePage>();
		builder.Services.AddSingleton<HomePageViewModel>();
		builder.Services.AddSingleton<AppInformationPage>();
		builder.Services.AddSingleton<AppInformationPageViewModel>();
		builder.Services.AddSingleton<ScanPage>();
		builder.Services.AddSingleton<ScanPageViewModel>();
		builder.Services.AddSingleton<ScanHistoryPage>();
		builder.Services.AddSingleton<ScanHistoryPageViewModel>();
		builder.Services.AddSingleton<ScanDetailPage>();
		builder.Services.AddSingleton<ScanDetailPageViewModel>();
		builder.Services.AddSingleton<AgentDetailPage>();
		builder.Services.AddSingleton<AgentDetailPageViewModel>();


		return builder.Build();
	}
}
