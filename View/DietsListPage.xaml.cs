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

        RefreshAll();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        RefreshAll();
    }

    private void RefreshAll()
    {
        diete = _globals.GetDiete();
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
            diete = _globals.GetCollectionsSearched(1, text);

            Refresh();
        }
    }
}