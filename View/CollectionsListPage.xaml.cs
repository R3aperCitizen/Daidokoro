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

        RefreshAll();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        RefreshAll();
    }

    private void RefreshAll()
    {
        collezioni = _globals.GetCollections();
        CollectionsList.ItemsSource = collezioni;
    }

    private void Refresh()
    {
        CollectionsList.ItemsSource = collezioni;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBar.Text;

        if(text == null) 
        { 
            RefreshAll(); 
        }
        else
        {
            collezioni = _globals.GetSearchedCollections(0, text);

            Refresh();
        }
    }
}