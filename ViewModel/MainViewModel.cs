using Daidokoro.Model;

namespace Daidokoro.ViewModel
{
    public class MainViewModel : IMainViewModel
    {
        private DBService _dbService = new();

        public DBService dbService { get { return _dbService; } }

        public bool InitDBSettings(DBCredentials dbs)
        {
            DBCredentials dbc = new()
            {
                Server = dbs.Server,
                Database = dbs.Database,
                User = dbs.User,
                Password = dbs.Password
            };
            return _dbService.TryConnection(dbc);
        }

        public List<Ricetta> GetRicette()
        {
            return _dbService.GetData<Ricetta>("ricetta");
        }
    }
}
