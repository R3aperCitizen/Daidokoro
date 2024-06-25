using Daidokoro.Model;
using Daidokoro.ViewModel;
using Newtonsoft.Json;

namespace Daidokoro.View;

public partial class RecipeCreationPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private readonly DisplayInfo _displayInfo;

    private List<string> passaggi;
    private byte[] selectedImage;
    private List<Model.IngredienteRicetta> ingredientiRicetta;
    private int IdRicetta;

    public RecipeCreationPage(IMainViewModel globals)
	{
		InitializeComponent();
        _globals = globals;
        _displayInfo = DeviceDisplay.MainDisplayInfo;
        SetBehaviours();

        passaggi = new();
        ingredientiRicetta = new();
    }

    private void ResetPage()
    {
        NomeRicetta.Text = string.Empty;
        Descrizione.Text = string.Empty;
        Passaggi.Text = "Lista dei passaggi:";
        Passaggio.Text = string.Empty;
        passaggi = new();
        SelectedImage.Source = null;
        selectedImage = null!;
        Difficulty.SelectedItem = Difficulty.ItemsSource[0];
        Time.Text = string.Empty;
        SearchBar.Text = string.Empty;
        IngredientsList.ItemsSource = null;
        Peso.Text = string.Empty;
        IdRicetta = 0;
        ingredientiRicetta = new();
        Ingredienti.Text = "Lista degli ingredienti:";
    }

    private void AddStep(object sender, EventArgs e)
    {
        if (Passaggio.Text != null && Passaggio.Text != string.Empty)
        {
            passaggi.Add(Passaggio.Text);
            Passaggi.Text = Passaggi.Text + "\r\n" + passaggi.Count().ToString() + ". " + Passaggio.Text;
            Passaggio.Text = string.Empty;
        }
    }

    private void SetBehaviours()
    {
        var screenHeight = _displayInfo.Height;
        var screenDensity = _displayInfo.Density;

        MainScroll.HeightRequest = (screenHeight / screenDensity) - 150;
        Difficulty.ItemsSource = new List<string>() { "1", "2", "3", "4", "5" };
        Difficulty.SelectedItem = Difficulty.ItemsSource[0];
    }

    private async void SelectImage(object sender, EventArgs e)
    {
        var response = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Seleziona una foto",
            FileTypes = FilePickerFileType.Images
        });

        if (response.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
            response.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
        {
            var stream = await response.OpenReadAsync();
            selectedImage = new byte[stream.Length];
            await stream.ReadAsync(selectedImage, 0, selectedImage.Length);
            stream = await response.OpenReadAsync();
            SelectedImage.Source = ImageSource.FromStream(() => stream);
        }
    }

    private async void CreateRecipe(object sender, EventArgs e)
    {
        if (NomeRicetta.Text != null && NomeRicetta.Text != string.Empty &&
            Descrizione.Text != null && Descrizione.Text != string.Empty &&
            passaggi.Count > 0 && 
            selectedImage != null && 
            Difficulty.SelectedItem != null &&
            Time.Text != null && Time.Text != string.Empty &&
            int.TryParse(Time.Text, out int time) && 
            int.TryParse(Difficulty.SelectedItem.ToString(), out int diff) &&
            ingredientiRicetta.Count > 0)
        {
            string passaggiJson = JsonConvert.SerializeObject(passaggi);
            await _globals.InsertNewRecipe(
            [
                new("Nome", NomeRicetta.Text),
                new("Descrizione", Descrizione.Text),
                new("Passaggi", passaggiJson),
                new("Foto", selectedImage),
                new("Difficolta", diff),
                new("Tempo", time)
            ]);
            IdRicetta = await _globals.GetInsertedRecipeId();

            ingredientiRicetta.ForEach(ir => ir.IdRicetta = IdRicetta);
            ingredientiRicetta.ForEach(async ir => await _globals.InsertRecipeIngredient([
                new("IdIngrediente", ir.IdIngrediente),
                new("IdRicetta", ir.IdRicetta),
                new("PesoInGrammi", ir.PesoInGrammi)
            ]));

            ResetPage();
            await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
        }
        else
        {
            await DisplayAlert("Impossibile creare la ricetta", "Completa tutti i campi!", "OK");
        }
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (SearchBar.Text != null && SearchBar.Text != string.Empty)
        {
            IngredientsList.ItemsSource = await _globals.GetSearchedIngredients(SearchBar.Text.ToLower());
        }
    }

    private void AddIngredient(object sender, EventArgs e)
    {
        if (IngredientsList.SelectedItem != null && 
            Peso.Text != null && 
            Peso.Text != string.Empty && 
            int.TryParse(Peso.Text, out int _peso))
        {
            Model.Ingrediente i = (Model.Ingrediente)IngredientsList.SelectedItem;
            ingredientiRicetta.Add(new Model.IngredienteRicetta
            {
                IdIngrediente = i.IdIngrediente,
                PesoInGrammi = _peso
            });
            Ingredienti.Text = Ingredienti.Text + "\r\n" + "● " + i.Nome + " - " + _peso.ToString() + " gr.";
        }
    }
}