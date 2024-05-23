﻿using Daidokoro.ViewModel;
using Microsoft.Extensions.Configuration;

namespace Daidokoro.View
{
    public partial class HomePage : ContentPage
    {
        // Global app variables
        private readonly IMainViewModel _globals;
        // Appsettings configuration
        private readonly IConfiguration _configuration;

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
        }

        private async void GoToUserPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
        }

        private async void GoToBrowsePage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(BrowsePage)}");
        }

        private async void GoToHomePage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        private async void GoToRicettaPage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(RicettaPage)}");
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            
        }
    }

}
