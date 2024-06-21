using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Daidokoro.ViewModel
{
    [QueryProperty(nameof(IdRicetta), nameof(IdRicetta))]
    public partial class RecipePageViewModel : ObservableObject
    {
        [ObservableProperty]
        string idRicetta;

        IConnectivity _connectivity;

        public RecipePageViewModel(IConnectivity connectivity)
        {
            _connectivity = connectivity;
        }

        [RelayCommand]
        Task Navigate() => Shell.Current.GoToAsync($"//{nameof(View.RecipePage)}?IdRicetta={IdRicetta}");
    }
}
