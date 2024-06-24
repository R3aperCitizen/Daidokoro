using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class UserPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    public UserPage(IMainViewModel globals)
	{
		InitializeComponent();
        _globals = globals;
	}

    protected async override void OnAppearing()
    {
        if (int.TryParse(await SecureStorage.Default.GetAsync("IdUtente"), out int IdUtente))
        {
            UserInfo.ItemsSource = await _globals.GetUserById(IdUtente);
        }
    }

    private async void LikesInfoButton_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Likes", "Premuto!", "OK");
    }

    private async void ReviewsInfoButton_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Recensioni", "Premuto!", "OK");
    }

    private async void RecipesInfoButton_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Ricette", "Premuto!", "OK");
    }

    private async void GoToRecipeCreationPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(RecipeCreationPage)}");
    }

    private void GoToCollectionCreationPage(object sender, EventArgs e)
    {

    }
}