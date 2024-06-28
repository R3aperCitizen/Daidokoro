using Daidokoro.Model;
using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class DietsListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private readonly DisplayInfo _displayInfo;

    public DietsListPage(IMainViewModel globals)
    {
        InitializeComponent();
        _globals = globals;
        _displayInfo = DeviceDisplay.MainDisplayInfo;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await SetBehaviours();
        DietsList.AsyncSource = _globals.GetCollectionsOrDiets(1);
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBar.Text;

        if (text == null)
        {
            DietsList.AsyncSource = _globals.GetCollectionsOrDiets(1);
        }
        else
        {
            string s1 = SearchBar.Text == string.Empty ? null! : SearchBar.Text;
            string s2 = null!;
            string s3 = null!;
            string s4 = "num";
            string s5 = null!;
            string s6 = null!;
            DietsList.AsyncSource = _globals.GetFilteredCollections(s1, s2, s3, s4, s5, 1, s6);
        }
    }

    private void SetFilters(object sender, EventArgs e)
    {
        FilterMenuButton.IsVisible = true;
        FilterMenu.IsVisible = false;
        var s1 = SearchBar.Text == string.Empty ? null! : SearchBar.Text;
        var s2 = DifficoltaCheck.IsChecked ? Math.Round(DifficoltaSlider.Value).ToString() : null!;
        var s3 = DataCheck.IsChecked ? DateOnly.FromDateTime(DataPicker.Date).ToString("o") : null!;
        var s4 = IMainViewModel.DietSort[SortPicker.SelectedItem.ToString()!];
        var s5 = NricetteCheck.IsChecked ? Math.Round(NricetteSlider.Value).ToString() : null!;
        var s6 = CategoryCheck.IsChecked ? ((CategoriaNutrizionale)CategoriesPicker.SelectedItem).Nome : null!;

        DietsList.AsyncSource = _globals.GetFilteredCollections(s1, s2, s3, s4, s5, 1, s6);             
    }

    private void OpenFilterMenu(object sender, EventArgs e)
    {
        FilterMenuButton.IsVisible = false;
        FilterMenu.IsVisible = true;  
    }

    private void DifficoltaSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        DifficoltaSlider.Value = Math.Round(DifficoltaSlider.Value);
        DifficoltaLabel.Text = DifficoltaSlider.Value.ToString();
    }

    private void NricetteSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        NricetteSlider.Value = Math.Round(NricetteSlider.Value);
        NricetteLabel.Text = NricetteSlider.Value.ToString();
    }

    private async Task SetBehaviours()
    {
        var screenHeight = _displayInfo.Height;
        var screenDensity = _displayInfo.Density;
        MainScroll.HeightRequest = (screenHeight / screenDensity) - 275;
        DifficoltaSlider.Maximum = 5;
        DifficoltaSlider.Minimum = 1;
        DifficoltaLabel.Text = DifficoltaSlider.Minimum.ToString();
        NricetteSlider.Maximum = 20;
        NricetteSlider.Minimum = 1;
        NricetteLabel.Text = NricetteSlider.Minimum.ToString();
        SortPicker.ItemsSource = IMainViewModel.DietSort.Keys.ToList();
        SortPicker.SelectedIndex = 0;
        CategoriesPicker.ItemsSource = await _globals.GetNutritionalCategories();
        CategoriesPicker.SelectedIndex = 0;
    }
}