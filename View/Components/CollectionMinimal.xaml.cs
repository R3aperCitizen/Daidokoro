namespace Daidokoro.View.Components;

public partial class CollectionMinimal : ContentView
{
	public CollectionMinimal()
	{
		InitializeComponent();
	}

    private async void GoToCollectionDietPage(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        await Shell.Current.GoToAsync($"//{nameof(CollectionDietPage)}?IdCollezione={button.AutomationId}");
    }
}