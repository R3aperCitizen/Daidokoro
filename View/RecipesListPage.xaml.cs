using Daidokoro.Model;
using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class RecipesListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private readonly DisplayInfo _displayInfo;

    private List<Model.Ricetta> ricette;
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

        await RefreshAll();
        Refresh();
    }
    
    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        await _SearchBar("NumeroLike");
    }

    private async Task _SearchBar(string orderby)
    {
        string text = SearchBar.Text;
        if (text == null || text == string.Empty)
        {
            await RefreshAll();
        }
        else
        {
            //query categoria nutrizionale
            ricette = await _globals.GetSearchedRecipes(text,orderby);
            Refresh();
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

    private async void SetFilters(object sender, EventArgs e)
    {
        FilterMenu.IsVisible = false;
        FilterMenuButton.IsVisible = true;
        if(!CheckDifficulty.IsChecked && !CheckCategories.IsChecked && !CheckTime.IsChecked)
        {
            await _SearchBar(IMainViewModel.sortings[SortPicker.SelectedItem.ToString()!]!);
        }
        else
        {
            
            ricette = await _globals.GetFilteredRecipes(
            CheckDifficulty.IsChecked ? Math.Round(DifficultySlider.Value).ToString() : null,
            CheckTime.IsChecked ? Math.Round(TimeSlider.Value).ToString() : null,
            IMainViewModel.sortings[SortPicker.SelectedItem.ToString()],
            CheckCategories.IsChecked ? ((CategoriaNutrizionale)CategoriesPicker.SelectedItem).Nome : null,
            SearchBar.Text);
                   
            Refresh();
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

    private async Task RefreshAll()
    {
        ricette = await _globals.GetRecipes();
    }

    private void Refresh()
    {
        RecipesList.ItemsSource = ricette;
    }
}