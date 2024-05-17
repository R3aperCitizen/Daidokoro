namespace Daidokoro.View;

public partial class UserPage : ContentPage
{
	public UserPage()
	{
		InitializeComponent();
	}
        
    private async void GoToUserPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
    }

    private void GoToSearchPage(object sender, EventArgs e)
    {

    }

    private async void GoToHomePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
}