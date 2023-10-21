using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.Models;
using AuthentIdMvpMobileApp.Services.Data;
using AuthentIdMvpMobileApp.ViewModels;

namespace AuthentIdMvpMobileApp.Views;

public partial class ScanHistoryPage : ContentPage
{
    AgentDataService _agentDataService;
	public ScanHistoryPage(ScanHistoryPageViewModel scanHistoryVm, IAgentService agentService)
	{
		InitializeComponent();
		BindingContext = scanHistoryVm;
        _agentDataService = (AgentDataService)agentService;
	}

    private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        var scan = ((VisualElement)sender).BindingContext as AuthentIdScan;
        AuthentIdAgent agent = new();

        if (scan == null)
            return;

        agent = await _agentDataService.GetAgent(scan.AgentId);

        Dictionary<string, object> scanInfo = new Dictionary<string, object>();
        scanInfo.Add("Scan", scan);
        scanInfo.Add("Agent", agent);
        await Shell.Current.GoToAsync($"{nameof(ScanDetailPage)}", true, scanInfo);

    }
}