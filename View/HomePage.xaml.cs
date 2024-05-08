using Daidokoro.Model;
using System.Collections.ObjectModel;

namespace Daidokoro.View
{
    public partial class HomePage : ContentPage
    {

        public HomePage()
        {
            InitializeComponent();

            Ricetta[] ricette = new[]
            {
                new Ricetta
                {
                    IdRicetta = 1,
                    Nome = "Porco",
                    Descrizione = "Madonna Puttana"
                }
            };
            SuggestedList.ItemsSource = ricette;
        }
    }

}
