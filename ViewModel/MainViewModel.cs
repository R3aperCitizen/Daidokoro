﻿using Daidokoro.Model;

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
                "collezione.*, categoria_nutrizionale.Nome AS NomeCategoria", 
                "JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria=collezione.IdCategoria WHERE Dieta = 0");
        }

        public List<Collezione> GetDiete()
        {
            return _dbService.GetData<Collezione>("collezione",
                "collezione.*, categoria_nutrizionale.Nome AS NomeCategoria",
                "JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria=collezione.IdCategoria WHERE Dieta = 1");
        }

        public List<Utente> GetUtente(int id)
        {
            return _dbService.GetData<Utente>("utente",
                "utente.*, IFNULL(temp_likes.Likes, 0) AS Likes, IFNULL(temp_reviews.ReviewCount, 0) AS ReviewCount, IFNULL(temp_recipes.RecipeCount, 0) AS RecipeCount",
                "LEFT JOIN (SELECT utente.IdUtente, COUNT(*) AS Likes FROM utente JOIN likes ON likes.IdUtente = utente.IdUtente GROUP BY likes.IdUtente) AS temp_likes " +
                "ON utente.IdUtente = temp_likes.IdUtente " +
                "LEFT JOIN (SELECT utente.IdUtente, COUNT(*) AS ReviewCount FROM utente JOIN valutazione ON valutazione.IdUtente = utente.IdUtente GROUP BY valutazione.IdUtente) AS temp_reviews " +
                "ON utente.IdUtente = temp_reviews.IdUtente " +
                "LEFT JOIN (SELECT utente.IdUtente, COUNT(*) AS RecipeCount FROM utente JOIN ricetta ON ricetta.IdUtente = utente.IdUtente GROUP BY ricetta.IdUtente) AS temp_recipes " +
                "ON utente.IdUtente = temp_recipes.IdUtente " +
                $"WHERE utente.IdUtente = {id}");
        }
    }
}
