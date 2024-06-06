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

        public List<Collezione> GetCollezioni()
        {
            return _dbService.GetData<Collezione>("collezione", 
                "collezione.*, categoria_nutrizionale.Nome AS NomeCategoria", 
                "JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria=collezione.IdCategoria WHERE Dieta = 0");
        }

        public List<Collezione> GetDiete()
        {
            return _dbService.GetData<Collezione>("collezione",
                "collezione.*, categoria_nutrizionale.Nome AS NomeCategoria",
                "JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria=collezione.IdCategoria WHERE Dieta = 1");
        }
    }
}
