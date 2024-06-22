using Daidokoro.ViewModel;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Daidokoro.View;

public partial class RecipePage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private readonly DisplayInfo _displayInfo;

    private Model.Ricetta ricetta;

    public RecipePage(IMainViewModel globals, RecipePageViewModel vm)
	{
        InitializeComponent();
        _globals = globals;
        _displayInfo = DeviceDisplay.MainDisplayInfo;

        BindingContext = vm;
        vm.PropertyChanged += OnPropertyChanged;
        ricetta = new();

        SetBehaviours();
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
            SetBehaviours();
        }
    }

    private void SetRecipe(int IdRicetta)
    {
        ricetta = _globals.GetRecipeById(IdRicetta);

        SetRecipeFields();
        SetRatingsBar();
    }

    private void SetRecipeFields()
    {
        Title.Text = ricetta.Nome;
        Image.Source = ricetta.Foto;
        Tags.Text = GetCategorieNutrizionali();
        Time.Text = $"{ricetta.Tempo} min.";
        Difficulty.Text = EmoticonDifficulty();
        Description.Text = ricetta.Descrizione;
        IngredientsList.Text = IngredientsToString();
        StepsList.Text = ParseStepsToJSON();

        Ratings.ItemsSource = _globals.GetRatingsByRecipe(ricetta.IdRicetta);
    }

    private void SetRatingsBar()
    {
        double screenWidth = (_displayInfo.Width / _displayInfo.Density) - (MainVSL.Padding.Right + MainVSL.Padding.Left);

        PositiveVoteButton.Text = $"🍋‍🟩 {ricetta.VotiPositivi} %";
        NegativeVoteButton.Text = $"🧅 {ricetta.VotiNegativi} %";

        PositiveVoteButton.WidthRequest = ricetta.VotiPositivi == 0 && ricetta.VotiNegativi == 0 
            ? screenWidth / 2 : screenWidth * (double)(ricetta.VotiPositivi / 100);
        NegativeVoteButton.WidthRequest = ricetta.VotiPositivi == 0 && ricetta.VotiNegativi == 0 
            ? screenWidth / 2 : screenWidth * (double)(ricetta.VotiNegativi / 100);
    }

    private void VotePositive(object sender, EventArgs e)
    {
        _globals.InsertRating(
        [
            new("IdUtente", 1),
            new("IdRicetta", ricetta.IdRicetta),
            new("Voto", true)
        ]);

        SetRecipe(ricetta.IdRicetta);
    }

    private void VoteNegative(object sender, EventArgs e)
    {
        _globals.InsertRating(
        [
            new("IdUtente", 1),
            new("IdRicetta", ricetta.IdRicetta),
            new("Voto", false)
        ]);

        SetRecipe(ricetta.IdRicetta);
    }

    public string EmoticonDifficulty()
    {
        string _difficulty = string.Empty;
        for (int i = 0; i < ricetta.Difficolta; i++)
        {
            _difficulty += "🌶️";
        }
        return _difficulty;
    }

    private string GetCategorieNutrizionali()
    {
        string categorie = "";
        var categories = _globals.GetNutritionalCategories(ricetta.IdRicetta);

        foreach (var item in categories) { categorie += item.Nome + "; "; }

        return categorie;
    }

    public string IngredientsToString()
    {
        string _ingredienti = string.Empty;
        foreach (Model.Ingrediente i in _globals.GetIngredients(ricetta.IdRicetta))
        {
            _ingredienti += "● "
                + i.Nome + " - "
                + i.Peso + " gr. \r\n";
        }
        return _ingredienti;
    }

    private string ParseStepsToJSON()
    {
        string formattedPassaggi = string.Empty;
        try
        {
            List<string> passaggi = JsonConvert.DeserializeObject<List<string>>(ricetta.Passaggi)!;
            for (int i=0; i<passaggi.Count(); i++)
            {
                formattedPassaggi += (i + 1).ToString() + ". "
                    + passaggi[i] + "\r\n";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return formattedPassaggi;
    }

    private void SetBehaviours()
    {
        var screenHeight = _displayInfo.Height;
        var screenWidth = _displayInfo.Width;
        var screenDensity = _displayInfo.Density;

        MainScroll.HeightRequest = (screenHeight / screenDensity) - 150;
        ImageBorder.WidthRequest = (screenWidth / screenDensity) - (MainVSL.Padding.Right + MainVSL.Padding.Left);
    }

    private void RefreshAll()
    {
        ricetta = new();
        SetRecipe(int.Parse(((RecipePageViewModel)BindingContext).IdRicetta));
    }
}