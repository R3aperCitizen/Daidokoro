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
                ricette = _globals.dbService.GetData<Ricetta>($"SELECT * FROM ricetta WHERE ricetta.Difficolta = {value}");
            }
            else
            {
                ricette = _globals.dbService.GetData<Ricetta>($"SELECT * FROM ricetta WHERE ricetta.Tempo = {value}");
            }
        }
        else
        {
            //query categoria nutrizionale
            ricette = _globals.dbService.GetData<Ricetta>( 
                $"SELECT DISTINCT ricetta.*\r\n" +
                $"FROM ricetta\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta=ricetta.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente=ingrediente.IdIngrediente\r\n" +
                $"JOIN categoria_nutrizionale ON ingrediente.IdCategoria=categoria_nutrizionale.IdCategoria\r\n" +
                $"WHERE LOWER(categoria_nutrizionale.Nome) LIKE \"%{text}%\"\r\n" +
                $"OR LOWER(ingrediente.Nome) LIKE \"%{text}%\"\r\n" +
                $"OR LOWER(ricetta.Nome) LIKE \"%{text}%\"\r\n" +
                $"LIMIT 10;"
            );
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