using Daidokoro.Model;
using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class RecipesListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private List<Ricetta> ricette;

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

    private async void GoToRecipePage(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await Shell.Current.GoToAsync($"//{nameof(RicettaPage)}?IdRicetta={button.AutomationId}");
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
                ricette = _globals.dbService.GetData<Ricetta>("ricetta", $"WHERE ricetta.Difficolta = {value}");
            }
            else
            {
                ricette = _globals.dbService.GetData<Ricetta>("ricetta", $"WHERE ricetta.Tempo = {value}");
            }
        }
        else
        {
            //query categoria nutrizionale
            ricette = _globals.dbService.GetData<Ricetta>("ricetta", "DISTINCT ricetta.*",
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = " +
                $"ricetta.IdRicetta\r\nJOIN ingrediente ON ingrediente_ricetta.IdIngrediente = " +
                $"ingrediente.IdIngrediente\r\nJOIN categoria_nutrizionale ON ingrediente.IdCategoria = " +
                $"categoria_nutrizionale.IdCategoria\r\nWHERE LOWER(categoria_nutrizionale.Nome) LIKE \"%{text}%\" " +
                $"OR LOWER(ingrediente.Nome) LIKE \"%{text}%\" OR LOWER(ricetta.Nome) LIKE \"%{text}%\" LIMIT 10");
        }

        RefreshRecipes();
    }

    private void RefreshAll()
    {
        ricette = _globals.GetRicette();
        RefreshRecipes();
    }

    private void RefreshRecipes()
    {
        RecipesList.ItemsSource = ricette;
    }
}