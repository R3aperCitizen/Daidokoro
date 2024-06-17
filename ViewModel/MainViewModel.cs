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
            return _dbService.GetData<Ricetta>("ricetta", "DISTINCT ricetta.*",
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = " +
                $"ricetta.IdRicetta\r\nJOIN ingrediente ON ingrediente_ricetta.IdIngrediente = " +
                $"ingrediente.IdIngrediente\r\nJOIN categoria_nutrizionale ON ingrediente.IdCategoria = " +
                $"categoria_nutrizionale.IdCategoria LIMIT 10");
        }

        public List<Ricetta> GetRicette(int IdCollezione)
        {
            return _dbService.GetData<Ricetta>("ricetta", 
                "ricetta.*", 
                $"JOIN ricetta_collezione ON ricetta_collezione.IdRicetta=ricetta.IdRicetta WHERE IdCollezione = {IdCollezione};");
        }

        public Collezione GetCollezione(int IdCollezione)
        {
            return _dbService.GetData<Collezione>("collezione",
                "collezione.*, categoria_nutrizionale.Nome AS NomeCategoria",
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria=collezione.IdCategoria WHERE IdCollezione = {IdCollezione};")[0];
        }

        public List<Collezione> GetCollezioni()
        {
            return _dbService.GetData<Collezione>("collezione", 
                "DISTINCT collezione.*, categoria_nutrizionale.Nome AS NomeCategoria, ricetta.Foto AS FotoRicetta ",
                "FROM collezione " +
                "JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = collezione.IdCategoria " +
                "JOIN ricetta_collezione ON ricetta_collezione.IdCollezione = collezione.IdCollezione " +
                "JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta " +
                "WHERE Dieta = 0 AND ricetta.IdRicetta = (" +
                "SELECT MIN(ricetta.IdRicetta) " +
                "FROM ricetta " +
                "JOIN ricetta_collezione ON ricetta_collezione.IdRicetta = ricetta.IdRicetta " +
                "JOIN collezione ON ricetta_collezione.IdCollezione = collezione.IdCollezione)");
        }

        public List<Collezione> GetDiete()
        {
            return _dbService.GetData<Collezione>("collezione",
                "DISTINCT collezione.*, categoria_nutrizionale.Nome AS NomeCategoria, ricetta.Foto AS FotoRicetta ",
                "FROM collezione " +
                "JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = collezione.IdCategoria " +
                "JOIN ricetta_collezione ON ricetta_collezione.IdCollezione = collezione.IdCollezione " +
                "JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta " +
                "WHERE Dieta = 1 AND ricetta.IdRicetta = (" +
                "SELECT MIN(ricetta.IdRicetta) " +
                "FROM ricetta " +
                "JOIN ricetta_collezione ON ricetta_collezione.IdRicetta = ricetta.IdRicetta " +
                "JOIN collezione ON ricetta_collezione.IdCollezione = collezione.IdCollezione)");
        }
    }
}
