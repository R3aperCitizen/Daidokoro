using Daidokoro.Model;
using Daidokoro.ViewModel;
using System.Numerics;

namespace Daidokoro.View;

public partial class RecipesListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private HashSet<Ricetta> ricette;

    public RecipesListPage(IMainViewModel globals)
    {
        InitializeComponent();
        _globals = globals;

        ricette = _globals.GetRicette().ToHashSet();
        Refresh();
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
        String text = SearchBar.Text; 
        if (int.TryParse(text,out int value))
        {
            if (value < 6) 
            {
                ricette = _globals.dbService.GetData<Ricetta>("ricetta", $"WHERE ricetta.Difficolta = {value}")
                    .ToHashSet();            
            }
            else
            {
                ricette = _globals.dbService.GetData<Ricetta>("ricetta", $"WHERE ricetta.Tempo = {value}")
                    .ToHashSet();
            }
           
        }
        else
        {
            //query categoria nutrizionale
            ricette = _globals.dbService.GetData<Ricetta>("ricetta", "ricetta.*",
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = " +
                $"ricetta.IdRicetta\r\nJOIN ingrediente ON ingrediente_ricetta.IdIngrediente = " +
                $"ingrediente.IdIngrediente\r\nJOIN categoria_nutrizionale ON ingrediente.IdCategoria = " +
                $"categoria_nutrizionale.IdCategoria\r\nWHERE categoria_nutrizionale.Nome LIKE \"%{text}%\"")
                    .ToHashSet();
            //query ingredienti
            ricette.UnionWith(_globals.dbService.GetData<Ricetta>("ricetta", "ricetta.*", $"JOIN ingrediente_ricetta " +
                $"ON ingrediente_ricetta.IdRicetta = ricetta.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"WHERE ingrediente.Nome LIKE \"%{text}%\"").ToHashSet());

            //query nome ricetta
            ricette.UnionWith(_globals.dbService.GetData<Ricetta>("ricetta","ricetta.*",$"WHERE ricetta.Nome LIKE \"%{text}%\" ")
                    .ToHashSet());
        }
        Refresh();
        return;
    }
    private void Refresh()
    {
        RecipesList.ItemsSource = ricette;
    }
}