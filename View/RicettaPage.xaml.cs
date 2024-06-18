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
        ricetta.IdRicetta = 0;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        SetRicetta(((RicettaPageViewModel)BindingContext).IdRicetta);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (ricetta.IdRicetta != 0)
        {
            SetRicetta(((RicettaPageViewModel)BindingContext).IdRicetta);
        }
    }

    private void SetRicetta(string Id)
    {
        int IdRicetta = int.Parse(Id);
        ricetta = _globals.dbService.GetData<Ricetta>($"SELECT * FROM ricetta WHERE IdRicetta = {Id};")[0];
        Recipe.ItemsSource = new List<Ricetta>() { ricetta };
        Ingredienti.Text = GetIngredienti(IdRicetta);
        Tags.Text = GetCategorieNutrizionali(IdRicetta);
    }

    private string GetIngredienti(int IdRicetta)
    {
        string listIngr = "";
        var Ingr = _globals.dbService.GetData<Ingrediente>(
            $"SELECT ingrediente.*\r\n" +
            $"FROM ingrediente\r\n" +
            $"JOIN ingrediente_ricetta ON ingrediente.IdIngrediente = ingrediente_ricetta.IdIngrediente\r\n" +  
            $"WHERE ingrediente_ricetta.IdRicetta = {IdRicetta};"
        );

        foreach ( var item in Ingr ) { listIngr += item.Nome + "\n"; }

        return listIngr;
    }

    private string GetCategorieNutrizionali(int IdRicetta)
    {
        string categorie = "";
        var categories = _globals.dbService.GetData<CategoriaNutrizionale>(
            $"SELECT DISTINCT categoria_nutrizionale.*\r\n" +
            $"FROM categoria_nutrizionale\r\n" +
            $"JOIN ingrediente ON categoria_nutrizionale.IdCategoria = ingrediente.IdCategoria\r\n" +
            $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
            $"WHERE ingrediente_ricetta.IdRicetta = {IdRicetta};"
        );

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