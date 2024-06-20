using Daidokoro.ViewModel;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Maui.Controls;

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
        SetScrollBehaviour();

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
        ricetta.Tags = GetCategorieNutrizionali(IdRicetta);
        ricetta.Ingredienti = _globals.GetIngredients(IdRicetta);
        Ratings.ItemsSource = _globals.GetRatingsByRecipe(IdRicetta);
        ParsePassaggiJSON();
        Recipe.ItemsSource = new List<Model.Ricetta>() { ricetta };
    }

    private string GetCategorieNutrizionali(int IdRicetta)
    {
        string categorie = "";
        var categories = _globals.GetNutritionalCategories(IdRicetta);

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

    private void SetScrollBehaviour()
    {
        var screenMetrics = DeviceDisplay.MainDisplayInfo;
        var screenHeight = screenMetrics.Height;
        var screenDensity = screenMetrics.Density;
        MainScroll.HeightRequest = (screenHeight / screenDensity) - 150;
    }

    private void RefreshAll()
    {
        ricetta = new();
        SetRicetta(((RecipePageViewModel)BindingContext).IdRicetta);
    }
}