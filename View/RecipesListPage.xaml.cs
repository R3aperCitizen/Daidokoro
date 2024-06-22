using Daidokoro.Model;
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
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await RefreshAll();
        await SetFilterMenuBehaviour();
    }
    
    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        await _SearchBar("NumeroLike");
    }

    private async Task _SearchBar(string orderby)
    {
        string text = SearchBar.Text.ToLower();
        if (text == null || text == string.Empty)
        {
            await RefreshAll();
        }
        else if (int.TryParse(text, out int value))
        {
            if (value < 6)
            {
                ricette = await _globals.GetRecipesByDifficulty(value, orderby);
            }
            else
            {
                ricette = await _globals.GetRecipesByTime(value, orderby);
            }
        }
        else
        {
            //query categoria nutrizionale
            ricette = await _globals.GetSearchedRecipes(text,orderby);
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

    private async void SetFilters(object sender, EventArgs e)
    {
        FilterMenu.IsVisible = false;
        FilterMenuButton.IsVisible = true;
        string query = 
            ($"SELECT t1.*" +
            $"\r\nFROM " +
            $"\r\n(SELECT ricetta.*" +
            $"\r\nFROM ricetta");
        if (CheckDifficulty.IsChecked)
        {
            query += $"\r\nWHERE ricetta.Difficolta = {Math.Round(DifficultySlider.Value)}";
            if (CheckTime.IsChecked)
            {
                query += " AND ";
            }
        }
        if(CheckTime.IsChecked) 
        {
            query += $"FLOOR(ricetta.Tempo / 10) * 10 = FLOOR({Math.Round(TimeSlider.Value)} / 10) * 10";
        }
        query += ") AS t1";
        if (CheckCategories.IsChecked)
        {
            query +=
            $"\r\nJOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = t1.IdRicetta" +
            $"\r\nJOIN ingrediente ON ingrediente.IdIngrediente = ingrediente_ricetta.IdIngrediente" +
            $"\r\nJOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = ingrediente.IdCategoria" +
            $"\r\nWHERE categoria_nutrizionale.Nome IN (\'{(string)CategoriesPicker.SelectedItem}\')";
        }
        if(!CheckCategories.IsChecked && !CheckDifficulty.IsChecked &&  !CheckTime.IsChecked) 
        {
            await _SearchBar(SortPicker.SelectedItem.ToString());
        } 
        else
        {
            query += $"\r\nORDER BY {SortPicker.SelectedItem.ToString()}";
            ricette = await _globals.dbService.GetData<Ricetta>(query);
            Refresh();
        }
       /* ricette = _globals.dbService.GetData<Ricetta>
        
            ($"SELECT t1.*" +
            $"\r\nFROM " +
            $"\r\n(SELECT ricetta.*" +  
            $"\r\nFROM ricetta" +
            $"\r\nWHERE ricetta.Difficolta = {Math.Round(DifficultySlider.Value)} AND FLOOR(ricetta.Tempo / 10) * 10 = FLOOR({Math.Round(TimeSlider.Value)} / 10) * 10) AS t1" +
            $"\r\nJOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = t1.IdRicetta" +
            $"\r\nJOIN ingrediente ON ingrediente.IdIngrediente = ingrediente_ricetta.IdIngrediente" +
            $"\r\nJOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = ingrediente.IdCategoria" +
            $"\r\nWHERE categoria_nutrizionale.Nome IN (\'{(string)CategoriesPicker.SelectedItem}\')" +
            $"\r\nGROUP BY t1.IdRicetta \r\n SORT BY ");
        Console.WriteLine($"{Math.Round(DifficultySlider.Value)}, '{(string)CategoriesPicker.SelectedItem}',{Math.Round(TimeSlider.Value)} ");
        Refresh();*/
    }

    private async Task SetFilterMenuBehaviour()
    {
        DifficultySlider.Maximum = 5;
        DifficultySlider.Minimum = 1;
        TimeSlider.Maximum = 100;
        TimeSlider.Minimum = 5;
        List<Model.CategoriaNutrizionale> categories = await _globals.GetNutritionalCategories();
        CategoriesPicker.ItemsSource = (from c in categories select c.Nome).ToList();
    }

    private async Task RefreshAll()
    {
        ricette = await _globals.GetRecipes();
        Refresh();
    }

    private void Refresh()
    {
        RecipesList.ItemsSource = ricette;
    }

    private void SortPicker_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}