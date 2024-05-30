using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Daidokoro.ViewModel
{
    [QueryProperty(nameof(IdRicetta), nameof(IdRicetta))]
    public partial class RicettaPageViewModel : ObservableObject
    {
        [ObservableProperty]
        string idRicetta;

        IConnectivity connectivity;

        public RicettaPageViewModel(IConnectivity connectivity)
        {
            this.connectivity = connectivity;
        }

        [RelayCommand]
        Task Navigate() => Shell.Current.GoToAsync($"//{nameof(View.RicettaPage)}?IdRicetta={IdRicetta}");
    }
}
