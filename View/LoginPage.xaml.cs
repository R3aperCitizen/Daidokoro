using Daidokoro.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace Daidokoro.View;

public partial class LoginPage : ContentPage
{
    // Global app variables
    private readonly IMainViewModel _globals;
    private readonly DisplayInfo _displayInfo;
    private byte[] selectedImage;

    public LoginPage(IMainViewModel globals)
    {
		InitializeComponent();
        _globals = globals;
        _displayInfo = DeviceDisplay.MainDisplayInfo;

        SetBehaviours();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        LoginSection.IsVisible = true;
        RegisterSection.IsVisible = false;
    }

    private void SetBehaviours()
    {
        var screenHeight = _displayInfo.Height;
        var screenDensity = _displayInfo.Density;

        MainScroll.HeightRequest = (screenHeight / screenDensity) - 400;
    }

    private void ResetPage()
    {
        Email.Text = string.Empty;
        Password.Text = string.Empty;
        Username.Text = string.Empty;
        EmailR.Text = string.Empty;
        PasswordR.Text = string.Empty;
        selectedImage = null!;
        SelectedImage.Source = null!;
    }

    private async void Login(object sender, EventArgs e)
    {
        string _email = Email.Text;
        string _password = Password.Text;

        if (_email != string.Empty &&
            new EmailAddressAttribute().IsValid(_email) &&
            _password != string.Empty)
        {
            if (await _globals.CanUserLogin(_email, _password))
            {
                await SecureStorage.Default.SetAsync("IdUtente", await _globals.GetLoggedUserId(_email));
                ResetPage();
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
        }
        else
        {
            await DisplayAlert("Impossibile accedere", "Completa tutti i campi!", "OK");
        }
    }

    private async void Register(object sender, EventArgs e)
    {
        string _username = Username.Text;
        string _email = EmailR.Text;
        string _password = PasswordR.Text;

        if (_username != string.Empty &&
            _email != string.Empty &&
            new EmailAddressAttribute().IsValid(_email) &&
            _password != string.Empty &&
            selectedImage != null)
        {
            if (await _globals.CanUserRegister(_email))
            {
                await _globals.RegisterUser(
                [
                    new("Username", _username),
                    new("Pwd", _password),
                    new("Email", _email),
                    new("Foto", selectedImage)
                ]);

                await SecureStorage.Default.SetAsync("IdUtente", await _globals.GetLoggedUserId(_email));
                ResetPage();
                await _globals.GiveRegisterObjective();
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
        }
        else
        {
            await DisplayAlert("Impossibile registrarsi", "Completa tutti i campi!", "OK");
        }
    }

    private void ShowRegisterSection(object sender, EventArgs e)
    {
        LoginSection.IsVisible = false;
        RegisterSection.IsVisible = true;
    }

    private void ShowLoginSection(object sender, EventArgs e)
    {
        LoginSection.IsVisible = true;
        RegisterSection.IsVisible = false;
    }

    private async void SelectImage(object sender, EventArgs e)
    {
        var response = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Seleziona una foto",
            FileTypes = FilePickerFileType.Images
        });

        if (response.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
            response.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) ||
            response.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase))
        {
            var stream = await response.OpenReadAsync();
            selectedImage = new byte[stream.Length];
            await stream.ReadAsync(selectedImage, 0, selectedImage.Length);
            stream = await response.OpenReadAsync();
            SelectedImage.Source = ImageSource.FromStream(() => stream);
        }
    }
}