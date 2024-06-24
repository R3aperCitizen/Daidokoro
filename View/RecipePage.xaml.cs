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
    private Model.VotiRicetta votiRicetta;
    private List<Model.Valutazione> valutazioni;

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

    private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        await RefreshAll();
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (ricetta.IdRicetta != 0)
        {
            await RefreshAll();
            SetBehaviours();
        }
    }

    private async Task SetRecipe(int IdRicetta)
    {
        ricetta = (await _globals.GetRecipeById(IdRicetta))[0];

        await SetRecipeFields();
        await SetRatingsBar();
        await SetRatings();
    }

    private async Task SetRecipeFields()
    {
        Title.Text = ricetta.Nome;
        Image.Source = ricetta.Foto;
        Tags.Text = await GetCategorieNutrizionali();
        Time.Text = $"{ricetta.Tempo} min.";
        Difficulty.Text = EmoticonDifficulty();
        Description.Text = ricetta.Descrizione;
        IngredientsList.Text = await IngredientsToString();
        StepsList.Text = ParseStepsToJSON();
    }

    private async Task SetRatingsBar()
    {
        double screenWidth = (_displayInfo.Width / _displayInfo.Density) - (MainVSL.Padding.Right + MainVSL.Padding.Left);
        votiRicetta = (await _globals.GetRatingsCountGroupByVoto(ricetta.IdRicetta, true))[0];
        decimal vpPercentage = votiRicetta.VotiPositivi == 0 && votiRicetta.VotiNegativi == 0 ? 
            0 : (votiRicetta.VotiPositivi / (votiRicetta.VotiPositivi + votiRicetta.VotiNegativi)) * 100;
        decimal vnPercentage = votiRicetta.VotiPositivi == 0 && votiRicetta.VotiNegativi == 0 ? 
            0 :(votiRicetta.VotiNegativi / (votiRicetta.VotiPositivi + votiRicetta.VotiNegativi)) * 100;

        PositiveVoteButton.Text = $"🍋‍🟩 {Math.Round(vpPercentage, 1)} %";
        NegativeVoteButton.Text = $"🧅 {Math.Round(vnPercentage, 1)} %";

        PositiveVoteButton.WidthRequest = screenWidth / 2;
        NegativeVoteButton.WidthRequest = screenWidth / 2;
    }

    private async Task SetRatings()
    {
        valutazioni = await _globals.GetRatingsById(ricetta.IdRicetta, true);
        Ratings.ItemsSource = valutazioni;
    }

    private async void VotePositive(object sender, EventArgs e)
    {
        await _globals.InsertRating(
        [
            new("IdRicetta", ricetta.IdRicetta),
            new("Voto", true)
        ], true);

        await SetRatingsBar();
        await SetRatings();
    }

    private async void VoteNegative(object sender, EventArgs e)
    {
        await _globals.InsertRating(
        [
            new("IdRicetta", ricetta.IdRicetta),
            new("Voto", false)
        ], true);

        await SetRatingsBar();
        await SetRatings();
    }

    private async void InsertReview(object sender, EventArgs e)
    {
        if (ReviewEntry.Text != null && ReviewEntry.Text != string.Empty)
        {
            await _globals.InsertReviewIfRatedByUser(ricetta.IdRicetta, ReviewEntry.Text, true);
            await SetRatings();
            ReviewEntry.Text = string.Empty;
        }
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

    private async Task<string> GetCategorieNutrizionali()
    {
        string categorie = "";
        var categories = await _globals.GetNutritionalCategories(ricetta.IdRicetta);

        foreach (var item in categories) { categorie += item.Nome + "; "; }

        return categorie;
    }

    public async Task<string> IngredientsToString()
    {
        string _ingredienti = string.Empty;
        foreach (Model.Ingrediente i in await _globals.GetIngredients(ricetta.IdRicetta))
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

    private async Task RefreshAll()
    {
        ricetta = new();
        await SetRecipe(int.Parse(((RecipePageViewModel)BindingContext).IdRicetta));
    }
}