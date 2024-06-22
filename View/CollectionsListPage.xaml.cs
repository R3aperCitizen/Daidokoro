using Daidokoro.ViewModel;

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
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await RefreshAll();
    }

    private async Task RefreshAll()
    {
        collezioni = await _globals.GetCollections();
        CollectionsList.ItemsSource = collezioni;
    }

    private void Refresh()
    {
        CollectionsList.ItemsSource = collezioni;
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBar.Text;

        if(text == null) 
        { 
            await RefreshAll();
        }
        else
        {
            collezioni = await _globals.GetSearchedCollections(0, text);

            Refresh();
        }
    }
}