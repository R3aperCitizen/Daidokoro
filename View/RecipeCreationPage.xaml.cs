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

    public RecipeCreationPage(IMainViewModel globals)
	{
		InitializeComponent();
        _globals = globals;
        _displayInfo = DeviceDisplay.MainDisplayInfo;
        SetBehaviours();

        passaggi = new();
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
            SelectedImage.HeightRequest = 250;
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
            int.TryParse(Difficulty.SelectedItem.ToString(), out int diff))
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

            await Shell.Current.GoToAsync($"//{nameof(UserPage)}");
        }
        else
        {
            await DisplayAlert("Impossibile creare la ricetta", "Completa tutti i campi!", "OK");
        }
    }
}