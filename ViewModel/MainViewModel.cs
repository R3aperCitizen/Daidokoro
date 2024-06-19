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

        public List<Ingrediente> GetIngredients(int IdRicetta)
        {
            return _dbService.GetData<Ingrediente>(
                $"SELECT ingrediente.*\r\n" +
                $"FROM ingrediente\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente.IdIngrediente = ingrediente_ricetta.IdIngrediente\r\n" +
                $"WHERE ingrediente_ricetta.IdRicetta = {IdRicetta};"
            );
        }

        public List<Ricetta> GetRecipes()
        {
            return _dbService.GetData<Ricetta>(
                $"CREATE VIEW r1 AS\r\n" +
                $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                $"FROM ricetta\r\n" +
                $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n" +
                $"GROUP BY ricetta.IdRicetta\r\n" +
                $"LIMIT 10;\r\n" +
                $"SELECT DISTINCT r1.*\r\n" +
                $"FROM r1\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = r1.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"ORDER BY NumeroLike DESC;\r\n" +
                $"DROP VIEW r1;"
           );
        }

        public List<Ricetta> GetRecipesByCollection(int IdCollezione)
        {
            return _dbService.GetData<Ricetta>(
                $"CREATE VIEW r1 AS\r\n" +
                $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                $"FROM ricetta\r\n" +
                $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdRicetta=ricetta.IdRicetta\r\n" +
                $"WHERE ricetta_collezione.IdCollezione = {IdCollezione}\r\n" +
                $"GROUP BY ricetta.IdRicetta\r\n;" +
                $"SELECT DISTINCT r1.*\r\n" +
                $"FROM r1\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = r1.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"ORDER BY NumeroLike DESC;\r\n" +
                $"DROP VIEW r1;"
            );
        }

        public List<Ricetta> GetRecipesByDifficulty(int Difficolta)
        {
            return _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE ricetta.Difficolta = {Difficolta}"
            );
        }

        public List<Ricetta> GetRecipesByTime(int Tempo)
        {
            return _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE ricetta.Tempo = {Tempo}"
            );
        }

        public Ricetta GetRecipeById(int IdRicetta)
        {
            return _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE IdRicetta = {IdRicetta};"
            )[0];
        }

        public Collezione GetCollectionById(int IdCollezione)
        {
            return _dbService.GetData<Collezione>(
                $"SELECT collezione.*, categoria_nutrizionale.Nome AS NomeCategoria\r\n" +
                $"FROM collezione\r\n" +
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria=collezione.IdCategoria\r\n" +
                $"WHERE IdCollezione = {IdCollezione};"
            )[0];
        }

        public List<Collezione> GetCollections()
        {
            return _dbService.GetData<Collezione>(
                $"SELECT c1.*, categoria_nutrizionale.Nome AS NomeCategoria, ricetta.Foto AS FotoRicetta\r\n" +
                $"FROM collezione AS c1\r\n" +
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = c1.IdCategoria\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdCollezione = c1.IdCollezione\r\n" +
                $"JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta\r\n" +
                $"WHERE Dieta = 0 AND ricetta.IdRicetta = (\r\n" +
                $"SELECT MIN(ricetta.IdRicetta)\r\n" +
                $"FROM ricetta\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdRicetta = ricetta.IdRicetta\r\n" +
                $"WHERE ricetta_collezione.IdCollezione = c1.IdCollezione);"
            );
        }

        public List<Collezione> GetDiets()
        {
            return _dbService.GetData<Collezione>(
                $"SELECT c1.*, categoria_nutrizionale.Nome AS NomeCategoria, ricetta.Foto AS FotoRicetta\r\n" +
                $"FROM collezione AS c1\r\n" +
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = c1.IdCategoria\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdCollezione = c1.IdCollezione\r\n" +
                $"JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta\r\n" +
                $"WHERE Dieta = 1 AND ricetta.IdRicetta = (\r\n" +
                $"SELECT MIN(ricetta.IdRicetta)\r\n" +
                $"FROM ricetta\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdRicetta = ricetta.IdRicetta\r\n" +
                $"WHERE ricetta_collezione.IdCollezione = c1.IdCollezione);"
            );
        }

        public List<Utente> GetUserById(int id)
        {
            return _dbService.GetData<Utente>(
                $"SELECT utente.*, IFNULL(temp_likes.Likes, 0) AS Likes, IFNULL(temp_reviews.ReviewCount, 0) AS ReviewCount, IFNULL(temp_recipes.RecipeCount, 0) AS RecipeCount\r\n" +
                $"FROM utente\r\n" +
                $"LEFT JOIN (SELECT utente.IdUtente, COUNT(*) AS Likes FROM utente JOIN likes ON likes.IdUtente = utente.IdUtente GROUP BY likes.IdUtente) AS temp_likes\r\n" +
                $"ON utente.IdUtente = temp_likes.IdUtente\r\n" +
                $"LEFT JOIN (SELECT utente.IdUtente, COUNT(*) AS ReviewCount FROM utente JOIN valutazione ON valutazione.IdUtente = utente.IdUtente GROUP BY valutazione.IdUtente) AS temp_reviews\r\n" +
                $"ON utente.IdUtente = temp_reviews.IdUtente\r\n" +
                $"LEFT JOIN (SELECT utente.IdUtente, COUNT(*) AS RecipeCount FROM utente JOIN ricetta ON ricetta.IdUtente = utente.IdUtente GROUP BY ricetta.IdUtente) AS temp_recipes\r\n" +
                $"ON utente.IdUtente = temp_recipes.IdUtente\r\n" +
                $"WHERE utente.IdUtente = {id};"
            );
        }

        public List<Ricetta> GetMonthRecipes()
        {
            return _dbService.GetData<Ricetta>(
                $"CREATE VIEW r1 AS\r\n" +
                $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                $"FROM ricetta\r\n" +
                $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n" +
                $"GROUP BY ricetta.IdRicetta;\r\n" +
                $"SELECT DISTINCT r1.*\r\n" +
                $"FROM r1\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = r1.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"ORDER BY NumeroLike DESC\r\n" +
                $"LIMIT 3;\r\n" +
                $"DROP VIEW r1;"
            );
        }

        public List<Ricetta> GetSearchedRecipes(string text)
        {
            return _dbService.GetData<Ricetta>(
                $"CREATE VIEW r1 AS\r\n" +
                $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                $"FROM ricetta\r\n" +
                $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n" +
                $"GROUP BY ricetta.IdRicetta;\r\n" +
                $"SELECT DISTINCT r1.*\r\n" +
                $"FROM r1\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = r1.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"JOIN categoria_nutrizionale ON ingrediente.IdCategoria=categoria_nutrizionale.IdCategoria\r\n" + 
                $"WHERE LOWER(categoria_nutrizionale.Nome) LIKE \"%{text}%\"\r\n" +
                $"OR LOWER(ingrediente.Nome) LIKE \"%{text}%\"\r\n" +
                $"OR LOWER(r1.Nome) LIKE \"%{text}%\"\r\n" +
                $"ORDER BY NumeroLike DESC\r\n" +
                $"LIMIT 10;\r\n" +
                $"DROP VIEW r1;"
            );
        }

        public List<Collezione> GetSearchedCollections(int dieta, string text)
        {
            return _dbService.GetData<Collezione>(
                $"CREATE VIEW c1_temp AS\r\n" +
                $"SELECT c1.*, categoria_nutrizionale.Nome AS NomeCategoria, ricetta.Foto AS FotoRicetta\r\n" +
                $"FROM collezione AS c1\r\n" +
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = c1.IdCategoria\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdCollezione = c1.IdCollezione\r\n" +
                $"JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta\r\n" +
                $"WHERE Dieta = {dieta} AND ricetta.IdRicetta = (\r\n" +
                $"SELECT MIN(ricetta.IdRicetta)\r\n" +
                $"FROM ricetta\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdRicetta = ricetta.IdRicetta\r\n" +
                $"WHERE ricetta_collezione.IdCollezione = c1.IdCollezione);\r\n" +
                $"SELECT DISTINCT c1_temp.*\r\n" +
                $"FROM c1_temp\r\n" +
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = c1_temp.IdCategoria\r\n" +
                $"JOIN ricetta_collezione ON c1_temp.IdCollezione = ricetta_collezione.IdCollezione\r\n" +
                $"JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta\r\n" +
                $"WHERE c1_temp.nome LIKE \"%{text}%\" OR categoria_nutrizionale.Nome LIKE \"%{text}%\" OR ricetta.Nome LIKE \"%{text}%\";\r\n" +
                $"DROP VIEW c1_temp;"
            );
        }

        public List<CategoriaNutrizionale> GetNutritionalCategory(int IdRicetta)
        {
            return _dbService.GetData<CategoriaNutrizionale>(
                $"SELECT DISTINCT categoria_nutrizionale.*\r\n" +
                $"FROM categoria_nutrizionale\r\n" +
                $"JOIN ingrediente ON categoria_nutrizionale.IdCategoria = ingrediente.IdCategoria\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"WHERE ingrediente_ricetta.IdRicetta = {IdRicetta};"
            );
        }
    }
}
