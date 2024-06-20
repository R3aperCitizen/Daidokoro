using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class RecipesListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private List<Model.Ricetta> ricette;

    public RecipesListPage(IMainViewModel globals)
    {
        InitializeComponent();
        _globals = globals;
        RefreshAll();
        SetFilterMenuBehaviour();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        RefreshAll();
    }
    
    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBar.Text.ToLower();
        if (text == null || text == string.Empty)
        {
            RefreshAll();
        }
        else if (int.TryParse(text, out int value))
        {
            if (value < 6)
            {
                ricette = _globals.GetRecipesByDifficulty(value);
            }
            else
            {
                ricette = _globals.GetRecipesByTime(value);
            }
        }
        else
        {
            //query categoria nutrizionale
            ricette = _globals.GetSearchedRecipes(text);
        }

        Refresh();
    }
    private void OpenFilterMenu(object sender, EventArgs e)
    {
        FilterMenu.IsVisible = true;
        FilterMenuButton.IsVisible = false;
    }

    private void DifficultySlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        DifficultyValue.Text = Math.Round(DifficultySlider.Value).ToString();
    }

    private void TimeSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        TimeValue.Text = Math.Round(TimeSlider.Value).ToString();
    }

    private void SetFilters(object sender, EventArgs e)
    {
        FilterMenu.IsVisible = false;
        FilterMenuButton.IsVisible = true;
    }

    private void SetFilterMenuBehaviour()
    {
        DifficultySlider.Maximum = 5;
        DifficultySlider.Minimum = 1;
        TimeSlider.Maximum = 100;
        TimeSlider.Minimum = 5;
        List<Model.CategoriaNutrizionale> categories = _globals.GetNutritionalCategories();
        CategoriesPicker.ItemsSource = (from c in categories select c.Nome).ToList();
    }

    private void RefreshAll()
    {
        ricette = _globals.GetRecipes();
        Refresh();
    }

    private void Refresh()
    {
        RecipesList.ItemsSource = ricette;
    }
}