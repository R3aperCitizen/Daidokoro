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
        SetFilterMenuBehaviour();
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
        if(!CheckDifficulty.IsChecked && !CheckCategories.IsChecked && !CheckTime.IsChecked)
        {
            await _SearchBar(SortPicker.SelectedItem.ToString()!);
        }
        else
        {
            ricette = await _globals.GetFilteredRecipes(
                CheckDifficulty.IsChecked ? Math.Round(DifficultySlider.Value).ToString() : null,
                CheckTime.IsChecked ? Math.Round(TimeSlider.Value).ToString() : null,
                IMainViewModel.sortings[SortPicker.SelectedItem.ToString()],
                CheckCategories.IsChecked ? IMainViewModel.categories[CategoriesPicker.SelectedItem.ToString()] : null);
            Refresh();
        }
    }

    private void SetFilterMenuBehaviour()
    {
        DifficultySlider.Maximum = 5;
        DifficultySlider.Minimum = 1;
        TimeSlider.Maximum = 100;
        TimeSlider.Minimum = 5;
        CategoriesPicker.ItemsSource = IMainViewModel.categories.Keys.ToList() ;
        SortPicker.ItemsSource = IMainViewModel.sortings.Keys.ToList() ;    
        CategoriesPicker.SelectedIndex = 0 ;
        SortPicker.SelectedIndex = 0;
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
}