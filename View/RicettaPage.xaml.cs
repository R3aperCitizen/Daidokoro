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
        Ingredienti.Text = getIngredienti("1");
        Passaggi.Text = ricetta.Passaggi;

    }

    private  string getIngredienti(string Idricetta)
    {
        string listIngr = "";
        var Ingr = _globals.dbService.GetData<Ingrediente>("ingrediente",
            "JOIN ingrediente_ricetta ON ingrediente.IdIngrediente = ingrediente_ricetta.IdIngrediente "  
            + " WHERE ingrediente_ricetta.IdRicetta = " + Idricetta);
        Console.WriteLine(Ingr.Count);
        foreach ( var item in Ingr )
        {
            listIngr += item.Nome + ", ";
        }

        return listIngr;
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
}