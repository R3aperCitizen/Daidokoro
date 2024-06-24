using Daidokoro.ViewModel;
using Microsoft.Extensions.Configuration;

namespace Daidokoro.View;

public partial class AuthCheckPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    // Appsettings configuration
    private readonly IConfiguration _configuration;

    public AuthCheckPage(IMainViewModel globals, IConfiguration configuration)
	{
		InitializeComponent();
        _globals = globals;
        _configuration = configuration;

        if (globals.dbService.dbCredentials.Server == string.Empty)
        {
            Model.DBCredentials dbs = _configuration.GetRequiredSection("DBCredentials").Get<Model.DBCredentials>()!;
            bool connection = _globals.InitDBSettings(dbs);
        }
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (await SecureStorage.Default.GetAsync("IdUtente") != null)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
        else
        {
           await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}