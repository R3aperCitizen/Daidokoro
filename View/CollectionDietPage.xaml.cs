using Daidokoro.ViewModel;
using System.ComponentModel;

namespace Daidokoro.View;

public partial class CollectionDietPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private readonly DisplayInfo _displayInfo;

    private List<Model.Ricetta> ricette;
    private Model.Collezione collezione;
    private Model.VotiRicetta votiCollezione;
    private List<Model.Valutazione> valutazioni;

    public CollectionDietPage(IMainViewModel globals, CollectionDietPageViewModel vm)
    {
        InitializeComponent();
        _globals = globals;
        _displayInfo = DeviceDisplay.MainDisplayInfo;
        SetBehaviours();

        BindingContext = vm;
        vm.PropertyChanged += OnPropertyChanged;
        ricette = new();
        collezione = new();
    }

    private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        await RefreshAll();
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (ricette.Any() && collezione.IdCollezione != 0)
        {
            await RefreshAll();
        }
    }

    private async Task SetCollezione(string Id)
    {
        int IdCollezione = int.Parse(Id);
        collezione = (await _globals.GetCollectionById(IdCollezione))[0];
        CollectionName.Text = collezione.Nome;
        CollectionDescription.Text = collezione.Descrizione;
        CollectionCategory.Text = collezione.NomeCategoria;
        CollectionDate.Text = "Data Creazione: " + collezione.DataCreazioneString;
        RecipesList.AsyncSource = _globals.GetRecipesByCollection(IdCollezione);

        if (collezione.Dieta)
        {
            RatingsBar.IsVisible = true;
            RatingSection.IsVisible = true;
            await SetRatingsBar();
            await SetRatings();
        }
        else
        {
            RatingsBar.IsVisible = false;
            RatingSection.IsVisible = false;
        }
    }

    private async Task SetRatingsBar()
    {
        double screenWidth = (_displayInfo.Width / _displayInfo.Density) - (MainVSL.Padding.Right + MainVSL.Padding.Left);
        votiCollezione = (await _globals.GetRatingsCountGroupByVoto(collezione.IdCollezione, false))[0];
        decimal vpPercentage = votiCollezione.VotiPositivi == 0 && votiCollezione.VotiNegativi == 0 ?
            0 : (votiCollezione.VotiPositivi / (votiCollezione.VotiPositivi + votiCollezione.VotiNegativi)) * 100;
        decimal vnPercentage = votiCollezione.VotiPositivi == 0 && votiCollezione.VotiNegativi == 0 ?
            0 : (votiCollezione.VotiNegativi / (votiCollezione.VotiPositivi + votiCollezione.VotiNegativi)) * 100;

        PositiveVoteButton.Text = $"🍋‍🟩 {Math.Round(vpPercentage, 1)} %";
        NegativeVoteButton.Text = $"🧅 {Math.Round(vnPercentage, 1)} %";

        PositiveVoteButton.WidthRequest = screenWidth / 2;
        NegativeVoteButton.WidthRequest = screenWidth / 2;
    }

    private async Task SetRatings()
    {
        valutazioni = await _globals.GetRatingsById(collezione.IdCollezione, false);
        Ratings.ItemsSource = valutazioni;
    }

    private async void VotePositive(object sender, EventArgs e)
    {
        await _globals.InsertRating(
        [
            new("IdCollezione", collezione.IdCollezione),
            new("Voto", true)
        ], false);

        await SetRatingsBar();
        await SetRatings();
    }

    private async void VoteNegative(object sender, EventArgs e)
    {
        await _globals.InsertRating(
        [
            new("IdCollezione", collezione.IdCollezione),
            new("Voto", false)
        ], false);

        await SetRatingsBar();
        await SetRatings();
    }

    private async void InsertReview(object sender, EventArgs e)
    {
        if (ReviewEntry.Text != null && ReviewEntry.Text != string.Empty)
        {
            await _globals.InsertReviewIfRatedByUser(collezione.IdCollezione, ReviewEntry.Text, false);
            await SetRatings();
            ReviewEntry.Text = string.Empty;
        }
    }

    private void SetBehaviours()
    {
        var screenHeight = _displayInfo.Height;
        var screenWidth = _displayInfo.Width;
        var screenDensity = _displayInfo.Density;

        MainScroll.HeightRequest = (screenHeight / screenDensity) - 150;
    }

    private async Task RefreshAll()
    {
        ricette = new();
        collezione = new();
        await SetCollezione(((CollectionDietPageViewModel)BindingContext).IdCollezione);
    }
}