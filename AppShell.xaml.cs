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

            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
            Routing.RegisterRoute(nameof(BrowsePage), typeof(BrowsePage));
            Routing.RegisterRoute(nameof(RicettaPage), typeof(RicettaPage));
            Routing.RegisterRoute(nameof(RecipesListPage), typeof(RecipesListPage));
        }
    }
}
