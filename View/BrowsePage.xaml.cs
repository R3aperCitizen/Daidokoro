using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class BrowsePage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    public BrowsePage(IMainViewModel globals)
	{
		InitializeComponent();
        _globals = globals;

        //List<Model.Ricetta> ricette = _globals.GetRicette();

        //SuggestedList.ItemsSource = ricette;
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
        await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
    }

}