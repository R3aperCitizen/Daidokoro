using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class LoginPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;

    public LoginPage(IMainViewModel globals)
    {
		InitializeComponent();
        _globals = globals;
    }

    private async void Login(object sender, EventArgs e)
    {
        string _email = Email.Text;
        string _password = Password.Text;

        if (_email != string.Empty && _password != string.Empty)
        {
            if (await _globals.CanUserLogin(_email, _password))
            {
                await SecureStorage.Default.SetAsync("IdUtente", await _globals.GetLoggedUserId(_email, _password));
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
        }
    }
}