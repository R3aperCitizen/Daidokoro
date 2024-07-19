using Daidokoro.Model;
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
		var idUtente = await SecureStorage.Default.GetAsync("IdUtente");

        objectiveList.Supplier = async () => await _globals.dbService.GetData<Obiettivo>(
		"WITH v1 AS(\r\n" +
		"SELECT DISTINCT obiettivo.*, obiettivo_ottenuto.DataOttenimento, obiettivo_ottenuto.IdUtente\r\n" +
		"FROM obiettivo\r\n" +
		"Left JOIN obiettivo_ottenuto\r\n" +
		"ON obiettivo_ottenuto.IdObiettivo = obiettivo.IdObiettivo)\r\n" +
		"SELECT v1.*\r\n" +
		"FROM v1\r\n" +
		$"WHERE IdUtente is null OR IdUtente = {idUtente}"
        );

		objectiveList.CachedRefresh();
    }
}