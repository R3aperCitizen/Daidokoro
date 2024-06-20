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
        SetFilterMenuBehaviour();
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
    private void Sdiff_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Vdiff.Text = Math.Round(Sdiff.Value).ToString();
    }

    private void Stime_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Vtime.Text = Math.Round(Stime.Value).ToString();
    }

    private void Bsubmt_Clicked(object sender, EventArgs e)
    {

    }

    private void SetFilterMenuBehaviour()
    {
        Sdiff.Maximum = 5;
        Sdiff.Minimum = 1;
        Sdiff.Value = Stime.Minimum;
        Stime.Maximum = 100;
        Stime.Minimum = 5;
        Stime.Value = Stime.Minimum;
        var vari = _globals.GetNutritionalCategories();
        Pcat.ItemsSource = (from c in vari select c.Nome).ToList();
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