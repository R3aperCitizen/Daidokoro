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

        var displayInfo = DeviceDisplay.MainDisplayInfo;
        MainScroll.HeightRequest = (displayInfo.Height / displayInfo.Density) - 150;
    }

    protected async override void OnAppearing()
    {
        int userId = await _globals.GetCurrentUserId();
        recipes.AsyncSource = _globals.GetRecipesByUser(userId);

        var result = await _globals.GetUserById(userId);
        this.BindingContext = result[0];
    }

    private async void LikesInfoButton_Clicked(object sender, EventArgs e)
    {
        int userId = await _globals.GetCurrentUserId();
        recipes.AsyncSource = _globals.GetLikedRecipes(userId);
        switchView.Index = 0;
    }

    private async void ReviewsInfoButton_Clicked(object sender, EventArgs e)
    {
        int userId = await _globals.GetCurrentUserId();
        recipes.AsyncSource = _globals.GetReviewedRecipes(userId);
        switchView.Index = 0;
    }

    private async void RecipesInfoButton_Clicked(object sender, EventArgs e)
    {
        int userId = await _globals.GetCurrentUserId();
        recipes.AsyncSource = _globals.GetRecipesByUser(userId);
        switchView.Index = 0;
    }

    private async void CollectionsInfoButton_Clicked(object sender, EventArgs e)
    {
        int userId = await _globals.GetCurrentUserId();
        collections.AsyncSource = _globals.GetCollectionsByUser(userId);
        switchView.Index = 1;
    }

    private async void GoToRecipeCreationPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(RecipeCreationPage)}");
    }

    private async void GoToCollectionCreationPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(CollectionCreationPage)}");
    }

    private async void GoToAchievementPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(ObjectiveListPage)}");
    }

    private async void Logout(object sender, EventArgs e)
    {
        SecureStorage.Default.Remove("IdUtente");
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}