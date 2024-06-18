namespace Daidokoro.View.Components;

public partial class InlineRecipe : ContentView
{
	public InlineRecipe()
	{
		InitializeComponent();
	}

    private async void GoToRecipePage(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await Shell.Current.GoToAsync($"//{nameof(RicettaPage)}?IdRicetta={button.AutomationId}");
    }
}