namespace Daidokoro.View.Components;

public partial class NavigationBar : ContentView
{
    public NavigationBar()
    {
        InitializeComponent();
        SetBehaviour();
    }

    private void SetBehaviour()
    {
        var screenMetrics = DeviceDisplay.MainDisplayInfo;
        var screenWidth = screenMetrics.Width;
        var screenDensity = screenMetrics.Density;
        NavBar.Spacing = (screenWidth / screenDensity) / 3.5;
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
}