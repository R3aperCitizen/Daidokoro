using Daidokoro.Model;

namespace Daidokoro.ViewModel
{
    public class MainViewModel : IMainViewModel
    {
        private DBService _dbService = new();

        public DBService dbService { get { return _dbService; } }

        public bool InitDBSettings(string _Server, string _Database, string _User, string _Password)
        {
            DBService.DBCredentials dbc = new(_Server, _Database, _User, _Password);
            return _dbService.TryConnection(dbc);
        }

        public List<Ricetta> GetRicette()
        {
            return _dbService.GetData<Ricetta>("ricetta");
        }
    }
}
