using Daidokoro.ViewModel;
namespace Daidokoro.View;

public partial class DietsListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    public DietsListPage(IMainViewModel globals)
    {
        InitializeComponent();
        _globals = globals;
        FilterSetUp();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
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

            DietsList.AsyncSource = _globals.getFilteredDiets(s1, s2, s3, s4, s5, 1);
        }
    }

    private void ButtonApply_Pressed(object sender, EventArgs e)
    {
        FilterMenuButton.IsVisible = true;
        filterMenu.IsVisible = false;
        var s1 = SearchBar.Text == string.Empty ? null! : SearchBar.Text;
        var s2 = DifficoltaCheck.IsChecked ? Math.Round(DifficoltaSlider.Value).ToString() : null!;
        var s3 = DataCheck.IsChecked ? DateOnly.FromDateTime(DataPicker.Date).ToString("o") : null!;
        var s4 = IMainViewModel.DietSort[SortPicker.SelectedItem.ToString()!];
        var s5 = NricetteCheck.IsChecked ? Math.Round(NricetteSlider.Value).ToString() : null!;

        DietsList.AsyncSource = _globals.getFilteredDiets(s1, s2, s3, s4, s5, 1);             
    }

    private void FilterMenuButton_Clicked(object sender, EventArgs e)
    {
        FilterMenuButton.IsVisible = false;
        filterMenu.IsVisible = true;  
    }

    private void FilterSetUp()
    {
        DifficoltaSlider.Maximum = 5;
        NricetteSlider.Maximum = 20;
        SortPicker.ItemsSource = IMainViewModel.DietSort.Keys.ToList();
        SortPicker.SelectedIndex = 0;

        
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
}