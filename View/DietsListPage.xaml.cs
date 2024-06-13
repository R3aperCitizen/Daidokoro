using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class DietsListPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private readonly List<Model.Collezione> diete;

    public DietsListPage(IMainViewModel globals)
    {
        InitializeComponent();
        _globals = globals;
        diete = new();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        diete.Clear();
        diete.AddRange(_globals.GetDiete());
        DietsList.ItemsSource = diete;
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
}