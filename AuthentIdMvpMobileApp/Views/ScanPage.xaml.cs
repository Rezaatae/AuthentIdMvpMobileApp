using AuthentIdMvpMobileApp.ViewModels;
using Plugin.NFC;

namespace AuthentIdMvpMobileApp.Views;

public partial class ScanPage : ContentPage
{
	private readonly ScanPageViewModel viewModel;
	public ScanPage(ScanPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
		this.viewModel = BindingContext as ScanPageViewModel;
	}

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
		viewModel.IsBusy = false;
		viewModel.Button_Clicked_StopListening();
    }
}