using AuthentIdMvpMobileApp.ViewModels;
using AuthentIdMvpMobileApp.Views;

namespace AuthentIdMvpMobileApp.Views;

public partial class ScanPage : ContentPage
{

	public ScanPage(ScanViewModel scanVm)
	{
		InitializeComponent();
		BindingContext = scanVm;
	}

}