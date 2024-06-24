using Daidokoro.View;
using Daidokoro.ViewModel;

namespace Daidokoro
{
    public partial class AppShell : Shell
    {
        private readonly IMainViewModel _globals;

        public AppShell()
        {
            InitializeComponent();
            _globals = new MainViewModel();

            Routing.RegisterRoute(nameof(AuthCheckPage), typeof(AuthCheckPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
            Routing.RegisterRoute(nameof(BrowsePage), typeof(BrowsePage));
            Routing.RegisterRoute(nameof(RecipePage), typeof(RecipePage));
            Routing.RegisterRoute(nameof(RecipeCreationPage), typeof(RecipeCreationPage));
            Routing.RegisterRoute(nameof(CollectionDietPage), typeof(CollectionDietPage));
            Routing.RegisterRoute(nameof(RecipesListPage), typeof(RecipesListPage));
            Routing.RegisterRoute(nameof(CollectionsListPage), typeof(CollectionsListPage));
            Routing.RegisterRoute(nameof(DietsListPage), typeof(DietsListPage));
        }
    }
}
