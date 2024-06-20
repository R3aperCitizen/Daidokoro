namespace Daidokoro.View.Controls;

public partial class RecipeFull : ContentView
{
	public RecipeFull()
	{
		InitializeComponent();
	}

    private async void GoToRecipePage(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await Shell.Current.GoToAsync($"//{nameof(RecipePage)}?IdRicetta={button.AutomationId}");
    }
}