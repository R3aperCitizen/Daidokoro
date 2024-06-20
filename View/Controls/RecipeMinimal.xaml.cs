namespace Daidokoro.View.Controls;

public partial class RecipeMinimal : ContentView
{
	public RecipeMinimal()
	{
		InitializeComponent();
	}

    private async void GoToRecipePage(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await Shell.Current.GoToAsync($"//{nameof(RecipePage)}?IdRicetta={button.AutomationId}");
    }
}