using Daidokoro.Model;
using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class RicettaPage : ContentPage
{
    private Ricetta ricetta;
    // Global app variables
    private readonly IMainViewModel _globals;
    public RicettaPage(IMainViewModel globals)
	{
        _globals = globals;
        InitializeComponent();

        ricetta = _globals.dbService.GetData<Ricetta>("ricetta", "WHERE IdRicetta = 1")[0];
        Titolo.Text = ricetta.Nome;
        Descrizione.Text = ricetta.Descrizione;
        Ingredienti.Text = "per ora pochi";
        Passaggi.Text = ricetta.Passaggi;

    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        
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

    private async void GoToRicettaPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(RicettaPage)}");
    }
}