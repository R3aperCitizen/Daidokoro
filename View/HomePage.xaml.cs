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

            //List<Model.Ricetta> ricette = _globals.GetRicette();

            //SuggestedList.ItemsSource = ricette;
        }

        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (_globals.dbService.dbCredentials == null!)
            {
                bool connection = _globals.InitDBSettings("127.0.0.1", "daidokoro", "root", "root");
                await DisplayAlert("Test", connection.ToString(), "OK");
            }
        }
    }

}
