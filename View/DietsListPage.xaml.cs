using Daidokoro.Model;
using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class DietsListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private List<Model.Collezione> diete;

    public DietsListPage(IMainViewModel globals)
    {
        InitializeComponent();
        _globals = globals;
        Snric.Maximum = 20;
        Sdiff.Maximum = 5;
        Stime.Maximum = 100;
        var vari = _globals.dbService.GetData<CategoriaNutrizionale>("SELECT categoria_nutrizionale.*\r\nFROM categoria_nutrizionale");
        Pcat.ItemsSource = (from c in vari select c.Nome).ToList();
        RefreshAll();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        RefreshAll();
    }

    private void RefreshAll()
    {
        diete = _globals.GetDiets();
        DietsList.ItemsSource = diete;
    }

    private void Refresh()
    {
        DietsList.ItemsSource = diete;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBar.Text;

        if (text == null)
        {
            RefreshAll();
        }
        else
        {
            diete = _globals.GetSearchedCollections(1, text);

            Refresh();
        }
    }

    private void Sdiff_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Vdiff.Text = Math.Round(Sdiff.Value).ToString();
    }

    private void Stime_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Vtime.Text = Math.Round(Stime.Value).ToString();
    }

    private void Snric_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Vnric.Text = Math.Round(Snric.Value).ToString();
    }

    private void Bsubmt_Clicked(object sender, EventArgs e)
    {

    }
}