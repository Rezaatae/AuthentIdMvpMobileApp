using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.ViewModels;

namespace AuthentIdMvpMobileApp.Views;

public partial class AgentDetailPage : ContentPage
{
	public AgentDetailPage(AgentDetailPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}