using Daidokoro.Model;
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

    private async void GoToUserPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
    }

    private async void GoToBrowsePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(BrowsePage)}");
    }

    private async void GoToHomePage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }

    private async void GoToCollectionDietPage(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await Shell.Current.GoToAsync($"//{nameof(CollectionDietPage)}?IdCollezione={button.AutomationId}");
    }

    private void RefreshAll()
    {
        collezioni = _globals.GetCollezioni();
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
            collezioni = _globals.GetCollectionsSearched(0, text);

            Refresh();
        }
    }
}