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