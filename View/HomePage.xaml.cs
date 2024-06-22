using Daidokoro.ViewModel;
using Microsoft.Extensions.Configuration;

namespace Daidokoro.View
{
    public partial class HomePage : ContentPage
    {
        // Global app variables
        private readonly IMainViewModel _globals;
        // Appsettings configuration
        private readonly IConfiguration _configuration;

        private List<Model.Ricetta> monthRecipes;

        public HomePage(IConfiguration configuration, IMainViewModel globals)
        {
            InitializeComponent();
            _configuration = configuration;
            _globals = globals;
            SetScrollBehaviour();

            if (globals.dbService.dbCredentials.Server == string.Empty)
            {
                Model.DBCredentials dbs = _configuration.GetRequiredSection("DBCredentials").Get<Model.DBCredentials>()!;
                bool connection = _globals.InitDBSettings(dbs);
            }
        }

        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            await SetMonthRecipes();
        }

        private void SetScrollBehaviour()
        {
            var screenMetrics = DeviceDisplay.MainDisplayInfo;
            var screenHeight = screenMetrics.Height;
            var screenDensity = screenMetrics.Density;
            MainScroll.HeightRequest = (screenHeight / screenDensity) - 250;
        }

        private async Task SetMonthRecipes()
        {
            monthRecipes = await _globals.GetMonthRecipes();
            MonthRecipe.ItemsSource = monthRecipes;
        }
    }

}
