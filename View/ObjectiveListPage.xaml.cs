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
		objectiveMinimalList.AsyncSource = _globals.dbService.GetData<Obiettivo>(
		"WITH v1 AS(\r\n" +
		"SELECT DISTINCT obiettivo.*, obiettivo_ottenuto.DataOttenimento, obiettivo_ottenuto.IdUtente\r\n" +
		"FROM obiettivo\r\n" +
		"Left JOIN obiettivo_ottenuto\r\n" +
		"ON obiettivo_ottenuto.IdObiettivo = obiettivo.IdObiettivo)\r\n" +
		"SELECT v1.*\r\n" +
		"FROM v1\r\n" +
		$"WHERE IdUtente is null OR IdUtente = {await SecureStorage.Default.GetAsync("IdUtente")}"


        );
    }
}