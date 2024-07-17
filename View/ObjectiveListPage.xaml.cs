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
		objectiveMinimalList.Source = await _globals.dbService.GetData<Obiettivo>(
			"SELECT *\r\n" +
			"FROM obiettivo\r\n" +
			"JOIN obiettivo_ottenuto ON obiettivo.IdObiettivo=obiettivo_ottenuto.IdObiettivo\r\n" +
			"WHERE IdUtente=1;"
		);
    }
}