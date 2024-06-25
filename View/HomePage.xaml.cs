using Daidokoro.ViewModel;

namespace Daidokoro.View
{
    public partial class HomePage : ContentPage
    {
        // Global app variables
        private readonly IMainViewModel _globals;

        private List<Model.Ricetta> monthRecipes;

        public HomePage(IMainViewModel globals)
        {
            InitializeComponent();
            _globals = globals;
            SetScrollBehaviour();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            MonthRecipe.AsyncSource = _globals.GetMonthRecipes();
        }

        private void SetScrollBehaviour()
        {
            var screenMetrics = DeviceDisplay.MainDisplayInfo;
            var screenHeight = screenMetrics.Height;
            var screenDensity = screenMetrics.Density;
            MainScroll.HeightRequest = (screenHeight / screenDensity) - 250;
        }
    }
}
