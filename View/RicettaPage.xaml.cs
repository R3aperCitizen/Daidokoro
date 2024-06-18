using Daidokoro.ViewModel;
using System.ComponentModel;

namespace Daidokoro.View;

public partial class RicettaPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    private Model.Ricetta ricetta;

    public RicettaPage(IMainViewModel globals, RicettaPageViewModel vm)
	{
        InitializeComponent();
        _globals = globals;

        BindingContext = vm;
        vm.PropertyChanged += OnPropertyChanged;
        ricetta = new Model.Ricetta();
        ricetta.IdRicetta = 0;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        SetRicetta(((RicettaPageViewModel)BindingContext).IdRicetta);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (ricetta.IdRicetta != 0)
        {
            SetRicetta(((RicettaPageViewModel)BindingContext).IdRicetta);
        }
    }

    private void SetRicetta(string Id)
    {
        int IdRicetta = int.Parse(Id);
        ricetta = _globals.GetRicetta(IdRicetta);
        Recipe.ItemsSource = new List<Model.Ricetta>() { ricetta };
        Ingredienti.Text = GetIngredienti(IdRicetta);
        Tags.Text = GetCategorieNutrizionali(IdRicetta);
    }

    private string GetIngredienti(int IdRicetta)
    {
        string listIngr = "";
        var Ingr = _globals.GetIngredienti(IdRicetta);

        foreach ( var item in Ingr ) { listIngr += item.Nome + "\n"; }

        return listIngr;
    }

    private string GetCategorieNutrizionali(int IdRicetta)
    {
        string categorie = "";
        var categories = _globals.GetCategorieNutrizionali(IdRicetta);

        foreach ( var item in categories ) { categorie += item.Nome + " "; }

        return categorie;
    }
}