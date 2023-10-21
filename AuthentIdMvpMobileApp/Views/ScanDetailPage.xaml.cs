using AuthentIdMvpMobileApp.Interfaces.Services;
using AuthentIdMvpMobileApp.ViewModels;

namespace AuthentIdMvpMobileApp.Views;

public partial class ScanDetailPage : ContentPage
{
	public ScanDetailPage(ScanDetailPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}