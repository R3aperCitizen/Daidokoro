using Daidokoro.ViewModel;

namespace Daidokoro.View
{
    public partial class HomePage : ContentPage
    {
        // Global app variables
        private readonly IMainViewModel _globals;

        public HomePage(IMainViewModel globals)
        {
            InitializeComponent();
            _globals = globals;
            SetScrollBehaviour();
            list.Supplier = async () => await _globals.GetMonthRecipes();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            list.Refresh();
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