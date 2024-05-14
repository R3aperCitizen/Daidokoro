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

            bool connection = _globals.InitDBSettings("192.168.167.7", "daidokoro", "root", "root");

            List<Model.Ricetta> ricette = _globals.GetRicette();

            SuggestedList.ItemsSource = ricette;
        }

        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            
        }
    }

}
