using Daidokoro.ViewModel;
using System.ComponentModel;
using System.Diagnostics;

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
        ricette = _globals.GetRicette(IdCollezione);
        CollectionName.Text = collezione.Nome;
        CollectionDescription.Text = collezione.Descrizione;
        CollectionCategory.Text = collezione.NomeCategoria;
        CollectionDate.Text = "Data Creazione: " + collezione.DataCreazioneString;
        RecipesList.ItemsSource = ricette;
    }
}