using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Daidokoro.ViewModel
{
    [QueryProperty(nameof(IdCollezione), nameof(IdCollezione))]
    public partial class CollectionDietPageViewModel : ObservableObject
    {
        [ObservableProperty]
        string idCollezione;

        IConnectivity connectivity;

        public CollectionDietPageViewModel(IConnectivity connectivity)
        {
            this.connectivity = connectivity;
        }

        [RelayCommand]
        Task Navigate() => Shell.Current.GoToAsync($"//{nameof(View.CollectionDietPage)}?IdCollezione={IdCollezione}");
    }
}
