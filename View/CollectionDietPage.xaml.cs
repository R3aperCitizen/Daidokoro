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

    private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        await RefreshAll();
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (ricette.Any() && collezione.IdCollezione != 0)
        {
            await RefreshAll();
        }
    }

    private async Task SetCollezione(string Id)
    {
        int IdCollezione = int.Parse(Id);
        List<Model.Collezione> c = await _globals.GetCollectionById(IdCollezione);
        collezione = c[0];
        ricette = await _globals.GetRecipesByCollection(IdCollezione);
        CollectionName.Text = collezione.Nome;
        CollectionDescription.Text = collezione.Descrizione;
        CollectionCategory.Text = collezione.NomeCategoria;
        CollectionDate.Text = "Data Creazione: " + collezione.DataCreazioneString;
        RecipesList.ItemsSource = ricette;
    }

    private async Task RefreshAll()
    {
        ricette = new();
        collezione = new();
        await SetCollezione(((CollectionDietPageViewModel)BindingContext).IdCollezione);
    }
}