using Daidokoro.Model;
using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class RecipesListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private readonly DisplayInfo _displayInfo;

    public RecipesListPage(IMainViewModel globals)
    {
        InitializeComponent();
        _globals = globals;
        _displayInfo = DeviceDisplay.MainDisplayInfo;
        SetBehaviours();
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        CategoriesPicker.ItemsSource = await _globals.GetNutritionalCategories();
        RecipesList.AsyncSource = _globals.GetRecipes();
    }
    
    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        _SearchBar("NumeroLike");
    }

    private void _SearchBar(string orderby)
    {
        string text = SearchBar.Text;
        if (text == null || text == string.Empty)
        {
            RecipesList.AsyncSource = _globals.GetRecipes();
        }
        else
        {
            //query categoria nutrizionale
            RecipesList.AsyncSource = _globals.GetSearchedRecipes(text,orderby);
        }        
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
        if(!CheckDifficulty.IsChecked && !CheckCategories.IsChecked && !CheckTime.IsChecked)
        {
            _SearchBar(SortPicker.SelectedItem.ToString()!);
        }
        else
        {

            RecipesList.AsyncSource = _globals.GetFilteredRecipes(
            CheckDifficulty.IsChecked ? Math.Round(DifficultySlider.Value).ToString() : null,
            CheckTime.IsChecked ? Math.Round(TimeSlider.Value).ToString() : null,
            IMainViewModel.sortings[SortPicker.SelectedItem.ToString()],
            CheckCategories.IsChecked ? ((CategoriaNutrizionale)CategoriesPicker.SelectedItem).Nome : null,
            SearchBar.Text);
        }
    }

    private void SetBehaviours()
    {
        var screenHeight = _displayInfo.Height;
        var screenDensity = _displayInfo.Density;
        MainScroll.HeightRequest = (screenHeight / screenDensity) - 275;
        DifficultySlider.Maximum = 5;
        DifficultySlider.Minimum = 1;
        TimeSlider.Maximum = 100;
        TimeSlider.Minimum = 5;
        SortPicker.ItemsSource = IMainViewModel.sortings.Keys.ToList();    
        CategoriesPicker.SelectedIndex = 0;
        SortPicker.SelectedIndex = 0;
    }
}