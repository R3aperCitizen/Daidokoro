using Daidokoro.ViewModel;

namespace Daidokoro.View;

public partial class ObjectiveListPage : ContentPage
{
	private readonly IMainViewModel _globals;
	public ObjectiveListPage(IMainViewModel globals)
	{
		_globals = globals;	
		InitializeComponent();
	}
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
		if (await SecureStorage.Default.GetAsync("IdUtente") is not null)
		{
			int IdUtente = int.Parse(await SecureStorage.Default.GetAsync("IdUtente"));

			objectiveList.Supplier = async () => await _globals.GetObjectives(IdUtente);

			objectiveList.CachedRefresh();
		}
    }
}