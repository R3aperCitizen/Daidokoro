using Daidokoro.Model;
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

        private List<Ricetta> monthRecipes;

        public HomePage(IConfiguration configuration, IMainViewModel globals)
        {
            InitializeComponent();
            _configuration = configuration;
            _globals = globals;

            if (globals.dbService.dbCredentials.Server == string.Empty)
            {
                Model.DBCredentials dbs = _configuration.GetRequiredSection("DBCredentials").Get<Model.DBCredentials>()!;
                bool connection = _globals.InitDBSettings(dbs);
            }

            SetMonthRecipes();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            SetMonthRecipes();
        }

        private void SetMonthRecipes()
        {
            monthRecipes = _globals.GetMonthRecipes();
            MonthRecipe.ItemsSource = monthRecipes;
        }
    }

}
