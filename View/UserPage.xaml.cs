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

    protected override void OnAppearing()
    {
        UserInfo.ItemsSource = _globals.GetUtente(2);
    }
}