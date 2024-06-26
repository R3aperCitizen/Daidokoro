using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class CollectionCreationPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private readonly DisplayInfo _displayInfo;

    private List<Model.RicettaCollezione> ricetteCollezioni;
    private List<Model.Ricetta> ricette;
    private Model.CategoriaNutrizionale categoria;
    private int IdCollezione;

    public CollectionCreationPage(IMainViewModel globals)
    {
        InitializeComponent();
        _globals = globals;
        _displayInfo = DeviceDisplay.MainDisplayInfo;
        SetBehaviours();

        ricetteCollezioni = new();
        ricette = new();
        categoria = new();
    }

    private void ResetPage()
    {
        NomeCollezione.Text = string.Empty;
        Descrizione.Text = string.Empty;
        SearchBar.Text = string.Empty;
        RecipesListView.ItemsSource = null;
        ricetteCollezioni = new();
        ricette = new();
        categoria = new();
        IdCollezione = 0;
    }

    private async void SetBehaviours()
    {
        var screenHeight = _displayInfo.Height;
        var screenDensity = _displayInfo.Density;

        MainScroll.HeightRequest = (screenHeight / screenDensity) - 150;
        CategoriesPicker.IsEnabled = false;
        CategoriesPicker.ItemsSource = await _globals.GetNutritionalCategories();
        CategoriesPicker.SelectedIndex = 0;
    }

    private void AddRecipe(object sender, EventArgs e)
    {
        if (RecipesListView.SelectedItem != null)
        {
            Model.Ricetta i = (Model.Ricetta)RecipesListView.SelectedItem;
            ricette.Add(i);
            ricetteCollezioni.Add(new Model.RicettaCollezione
            {
                IdRicetta = i.IdRicetta
            });
            RecipesList.Source = ricette;
        }
    }

    private async void CreateCollection(object sender, EventArgs e)
    {
        if (NomeCollezione.Text != null && NomeCollezione.Text != string.Empty &&
            Descrizione.Text != null && Descrizione.Text != string.Empty &&
            ricetteCollezioni.Count > 0)
        {
            await _globals.InsertNewCollection(
            [
                new("Nome", NomeCollezione.Text),
                new("Descrizione", Descrizione.Text),
                new("Dieta", IsDieta.IsChecked),
                new("IdCategoria", IsDieta.IsChecked ? categoria.IdCategoria : 3)
            ]);
            IdCollezione = await _globals.GetInsertedCollectionId();

            ricetteCollezioni.ForEach(rc => rc.IdCollezione = IdCollezione);
            ricetteCollezioni.ForEach(async rc => await _globals.InsertCollectionRecipe([
                new("IdRicetta", rc.IdRicetta),
                new("IdCollezione", rc.IdCollezione)
            ]));

            ResetPage();
            await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
        }
        else
        {
            await DisplayAlert("Impossibile creare la collezione", "Completa tutti i campi!", "OK");
        }
    }

    private void IsDiet(object sender, CheckedChangedEventArgs e)
    {
        CategoriesPicker.IsEnabled = IsDieta.IsChecked;
        if (CategoriesPicker.IsEnabled)
        {
            categoria = (Model.CategoriaNutrizionale)CategoriesPicker.SelectedItem;
        }
        else
        {
            categoria = new();
        }
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (SearchBar.Text != null && SearchBar.Text != string.Empty)
        {
            RecipesListView.ItemsSource = await _globals.GetSearchedRecipes(SearchBar.Text.ToLower(), categoria.IdCategoria);
        }
    }

    private void SetCategoria(object sender, EventArgs e)
    {
        categoria = (Model.CategoriaNutrizionale)CategoriesPicker.SelectedItem;
    }
}