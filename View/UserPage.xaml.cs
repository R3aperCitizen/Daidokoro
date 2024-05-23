using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class UserPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    public UserPage(IMainViewModel globals)
	{
		InitializeComponent();
        _globals = globals;
	}
        
    private async void GoToUserPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
    }

    private async void GoToBrowsePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(BrowsePage)}");
    }

    private async void GoToHomePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
}