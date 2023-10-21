using AuthentIdMvpMobileApp.ViewModels;

namespace AuthentIdMvpMobileApp.Views;

public partial class HomePage : ContentPage
{
    private readonly HomePageViewModel viewModel;
    public HomePage(HomePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        this.viewModel = BindingContext as HomePageViewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        viewModel.UserFirstName = Preferences.Get("CurrentUserFirstName", "[User Name]");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.UserFirstName = Preferences.Get("CurrentUserFirstName", "[User Name]");
    }
}