using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class DietsListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    public DietsListPage(IMainViewModel globals)
    {
        InitializeComponent();
        _globals = globals;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        DietsList.AsyncSource = _globals.GetDiets();
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBar.Text;

        if (text == null)
        {
            DietsList.AsyncSource = _globals.GetDiets();
        }
        else
        {
            DietsList.AsyncSource = _globals.GetSearchedCollections(1, text);
        }
    }
}