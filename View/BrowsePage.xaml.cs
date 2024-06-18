using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class BrowsePage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    public BrowsePage(IMainViewModel globals)
	{
		InitializeComponent();
        _globals = globals;

        //List<Model.Ricetta> ricette = _globals.GetRicette();

        //SuggestedList.ItemsSource = ricette;
    }

    private async void GoToRecipesList(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(RecipesListPage)}");
    }

    private async void GoToCollectionsList(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(CollectionsListPage)}");
    }

    private async void GoToDietsList(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(DietsListPage)}");
    }
}