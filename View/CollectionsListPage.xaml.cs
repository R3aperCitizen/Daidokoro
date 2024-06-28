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
        CollectionsList.AsyncSource = _globals.GetCollectionsOrDiets(0);
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string text = SearchBar.Text;

        if(text == null) 
        {
            CollectionsList.AsyncSource = _globals.GetCollectionsOrDiets(0);
        }
        else
        {
            string s1 = SearchBar.Text == string.Empty ? null! : SearchBar.Text;
            string s2 = null!;
            string s3 = null!;
            string s4 = "num";
            string s5 = null!;

            CollectionsList.AsyncSource = _globals.getFilteredDiets(s1, s2, s3, s4, s5, 0);
        }
    }
}