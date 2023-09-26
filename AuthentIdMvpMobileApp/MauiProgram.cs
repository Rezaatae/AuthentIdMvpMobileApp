using AuthentIdMvpMobileApp.ViewModels;
using Microsoft.Extensions.Logging;
using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.Views;
#if ANDROID || iOS
using AuthentIdMvpMobileApp.Platforms.Android;
#endif
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

#if ANDROID || iOS
        builder.Services.AddTransient<INfcService, NfcService>();
#endif
#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddTransient<ScanPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<ScanHistoryPage>();
        builder.Services.AddTransient<ScanViewModel>();
        return builder.Build();
	}
}
