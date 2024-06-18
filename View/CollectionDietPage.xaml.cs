using Daidokoro.ViewModel;
using System.ComponentModel;

namespace Daidokoro.View;

public partial class CollectionDietPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private List<Model.Ricetta> ricette;
    private Model.Collezione collezione;

    public CollectionDietPage(IMainViewModel globals, CollectionDietPageViewModel vm)
    {
        InitializeComponent();
        _globals = globals;
        BindingContext = vm;
        vm.PropertyChanged += OnPropertyChanged;
        ricette = new();
        collezione = new();
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ricette = new();
        collezione = new();
        SetCollezione(((CollectionDietPageViewModel)BindingContext).IdCollezione);
    }

    private void SetCollezione(string Id)
    {
        int IdCollezione = int.Parse(Id);
        collezione = _globals.GetCollezione(IdCollezione);
        ricette = _globals.GetRicetteByCollection(IdCollezione);
        CollectionName.Text = collezione.Nome;
        CollectionDescription.Text = collezione.Descrizione;
        CollectionCategory.Text = collezione.NomeCategoria;
        CollectionDate.Text = "Data Creazione: " + collezione.DataCreazioneString;
        RecipesList.ItemsSource = ricette;
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
}