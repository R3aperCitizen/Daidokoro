using Daidokoro.Model;
using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class RicettaPage : ContentPage
{
    private Ricetta ricetta;
    // Global app variables
    private readonly IMainViewModel _globals;
    public RicettaPage(IMainViewModel globals)
	{
        _globals = globals;
        InitializeComponent();

        ricetta = 
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        
    }
}