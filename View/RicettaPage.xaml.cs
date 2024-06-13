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
        _globals = globals;

        BindingContext = vm;
        vm.PropertyChanged += OnPropertyChanged;
        ricetta = new Ricetta();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        setRicetta(((RicettaPageViewModel)BindingContext).IdRicetta);
    }

    private void setRicetta(string Id)
    {
        int IdRicetta = int.Parse(Id);
        ricetta = _globals.dbService.GetData<Ricetta>("ricetta", $"WHERE IdRicetta = {Id};")[0];
        Recipe.ItemsSource = new List<Ricetta>() { ricetta };
        Ingredienti.Text = getIngredienti(IdRicetta);
        Tags.Text = getCategorieNutrizionali(IdRicetta);
    }

    private string getIngredienti(int IdRicetta)
    {
        string listIngr = "";
        var Ingr = _globals.dbService.GetData<Ingrediente>("ingrediente",
            "ingrediente.*",
            "JOIN ingrediente_ricetta ON ingrediente.IdIngrediente = ingrediente_ricetta.IdIngrediente\r\n" +  
            $"WHERE ingrediente_ricetta.IdRicetta = {IdRicetta};");

        foreach ( var item in Ingr ) { listIngr += item.Nome + "\n"; }

        return listIngr;
    }

    private string getCategorieNutrizionali(int IdRicetta)
    {
        string categorie = "";
        var categories = _globals.dbService.GetData<CategoriaNutrizionale>("categoria_nutrizionale", 
          "DISTINCT categoria_nutrizionale.*",
          "JOIN ingrediente ON categoria_nutrizionale.IdCategoria = ingrediente.IdCategoria\r\n" +
          "JOIN ingrediente_ricetta ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
          $"WHERE ingrediente_ricetta.IdRicetta = {IdRicetta};");

        foreach ( var item in categories ) { categorie += item.Nome + " "; }

        return categorie;
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