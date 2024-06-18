using Daidokoro.Model;
using Daidokoro.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Daidokoro.View;

public partial class CollectionsListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private List<Model.Collezione> collezioni;

    public CollectionsListPage(IMainViewModel globals)
	{
		InitializeComponent();
        _globals = globals;

        RefreshAll();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        RefreshAll();
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

    private async void GoToCollectionDietPage(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await Shell.Current.GoToAsync($"//{nameof(CollectionDietPage)}?IdCollezione={button.AutomationId}");
    }

    private void RefreshAll()
    {
        collezioni = _globals.GetCollezioni();
        CollectionsList.ItemsSource = collezioni;
    }

    private void Refresh(List<Collezione> items)
    {
        CollectionsList.ItemsSource = items;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBar.Text;

        if(text == null) { RefreshAll(); }
        else
        {
            Refresh(_globals.dbService.GetData<Collezione>
               ($"SELECT Distinct collezione.* \r\nFROM collezione\r\nJOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = " +
               $"collezione.IdCategoria\r\nJOIN ricetta_collezione ON collezione.IdCollezione = ricetta_collezione.IdCollezione\r\nJOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta\r\nWHERE collezione.nome like \"%{text}%\"" +
               $" OR categoria_nutrizionale.Nome like \"%{text}%\" OR ricetta.Nome like \"%{text}%\"\r\n\r\n\r\n\r\n\r\n\r\n"));
        }
    }
}