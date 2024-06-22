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
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await RefreshAll();
    }

    private async Task RefreshAll()
    {
        diete = await _globals.GetDiets();
        DietsList.ItemsSource = diete;
    }

    private void Refresh()
    {
        DietsList.ItemsSource = diete;
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBar.Text;

        if (text == null)
        {
            await RefreshAll();
        }
        else
        {
            diete = await _globals.GetSearchedCollections(1, text);

            Refresh();
        }
    }
}