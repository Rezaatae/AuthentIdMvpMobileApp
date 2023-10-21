using AuthentIdMvpMobileApp.Views;

namespace AuthentIdMvpMobileApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(ScanDetailPage), typeof(ScanDetailPage));
		Routing.RegisterRoute(nameof(AgentDetailPage), typeof(AgentDetailPage));
	}
}
