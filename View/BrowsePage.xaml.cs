namespace Daidokoro.View;

public partial class BrowsePage : ContentPage
{
	public BrowsePage()
	{
		InitializeComponent();
	}
    private async void GoToUserPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
    }

    private async void GoToSearchPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(BrowsePage)}");
    }

    private void GoToHomePage(object sender, EventArgs e)
    {

    }

}