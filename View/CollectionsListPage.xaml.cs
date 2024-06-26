using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class CollectionsListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    public CollectionsListPage(IMainViewModel globals)
	{
		InitializeComponent();
        _globals = globals;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        CollectionsList.AsyncSource = _globals.GetCollections();
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBar.Text;

        if(text == null) 
        {
            CollectionsList.AsyncSource = _globals.GetCollections();
        }
        else
        {
            CollectionsList.AsyncSource = _globals.GetSearchedCollections(0, text);
        }
    }
}