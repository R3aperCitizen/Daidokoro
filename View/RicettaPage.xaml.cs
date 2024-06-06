using Daidokoro.Model;
using Daidokoro.ViewModel;
using System.ComponentModel;

namespace Daidokoro.View;

public partial class RicettaPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    private Ricetta ricetta;

    public RicettaPage(IMainViewModel globals, RicettaPageViewModel vm)
	{
        InitializeComponent();
        BindingContext = vm;
        vm.PropertyChanged += OnPropertyChanged;
        _globals = globals;
        ricetta = new Ricetta();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        setRicetta(((RicettaPageViewModel)BindingContext).IdRicetta);
    }

    private void setRicetta(string Id)
    {
        int IdRicetta = int.Parse(Id);
        ricetta = _globals.dbService.GetData<Ricetta>("ricetta", "WHERE IdRicetta = " + IdRicetta)[0];
        Nome.Text = ricetta.Nome;
        Descrizione.Text = ricetta.Descrizione;
        Ingredienti.Text = getIngredienti(IdRicetta);
        Passaggi.Text = ricetta.Passaggi;
        Tags.Text = getCategorieNutrizionali(IdRicetta);
    }

    private string getIngredienti(int IdRicetta)
    {
        string listIngr = "";
        var Ingr = _globals.dbService.GetData<Ingrediente>("ingrediente",
            "ingrediente.*",
            "JOIN ingrediente_ricetta ON ingrediente.IdIngrediente = ingrediente_ricetta.IdIngrediente "  
            + "WHERE ingrediente_ricetta.IdRicetta = " + IdRicetta);
        foreach ( var item in Ingr )
        {
            listIngr += item.Nome + "\n";
        }

        return listIngr;
    }

    private string getCategorieNutrizionali(int idRicetta)
    {
        string ingredient = "";
        var categories = _globals.dbService.GetData<Categorianutrizionale>("categorianutrizionale", "Distinct categorianutrizionale.*",
          "JOIN ingrediente ON categorianutrizionale.IdCategoria = ingrediente.IdCategoria\r\n" +
          "JOIN ingrediente_ricetta ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
          "WHERE ingrediente_ricetta.IdRicetta = 1\r\n\r\n");

        foreach ( var item in categories ) { ingredient += item.Nome + " "; }

        return ingredient;
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