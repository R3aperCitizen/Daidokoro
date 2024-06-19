using Daidokoro.ViewModel;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Daidokoro.View;

public partial class RecipePage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    private Model.Ricetta ricetta;

    public RecipePage(IMainViewModel globals, RecipePageViewModel vm)
	{
        InitializeComponent();
        _globals = globals;

        BindingContext = vm;
        vm.PropertyChanged += OnPropertyChanged;
        ricetta = new();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        RefreshAll();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (ricetta.IdRicetta != 0)
        {
            RefreshAll();
        }
    }

    private void SetRicetta(string Id)
    {
        int IdRicetta = int.Parse(Id);
        ricetta = _globals.GetRecipeById(IdRicetta);
        ricetta.Ingredienti = _globals.GetIngredients(IdRicetta);
        ricetta.Tags = GetCategorieNutrizionali(IdRicetta);
        ParsePassaggiJSON();
        Recipe.ItemsSource = new List<Model.Ricetta>() { ricetta };
    }

    private string GetCategorieNutrizionali(int IdRicetta)
    {
        string categorie = "";
        var categories = _globals.GetNutritionalCategory(IdRicetta);

        foreach ( var item in categories ) { categorie += item.Nome + "; "; }

        return categorie;
    }

    private void ParsePassaggiJSON()
    {
        string formattedPassaggi = string.Empty;
        try
        {
            List<string> passaggi = JsonConvert.DeserializeObject<List<string>>(ricetta.Passaggi);
            for (int i=0; i<passaggi.Count(); i++)
            {
                formattedPassaggi += (i+1).ToString() + ". " 
                    + passaggi[i] + "\r\n";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        ricetta.Passaggi = formattedPassaggi;
    }

    private void RefreshAll()
    {
        ricetta = new();
        SetRicetta(((RecipePageViewModel)BindingContext).IdRicetta);
    }
}