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

        public async Task<List<Ingrediente>> GetIngredients(int IdRicetta)
        {
            return await _dbService.GetData<Ingrediente>(
                $"SELECT ingrediente.*, ingrediente_ricetta.PesoInGrammi AS Peso\r\n" +
                $"FROM ingrediente\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente.IdIngrediente = ingrediente_ricetta.IdIngrediente\r\n" +
                $"WHERE ingrediente_ricetta.IdRicetta = {IdRicetta};"
            );
        }

        public async Task<List<Ricetta>> GetRecipes()
        {
            return await _dbService.GetData<Ricetta>(
                $"DROP VIEW IF EXISTS r1;\r\n" +
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
                $"ORDER BY NumeroLike DESC;"
           );
        }

        public async Task<List<Ricetta>> GetRecipesByCollection(int IdCollezione)
        {
            return await _dbService.GetData<Ricetta>(
                $"DROP VIEW IF EXISTS r1;\r\n" +
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
                $"ORDER BY NumeroLike DESC;"
            );
        }

        public async Task<List<Ricetta>> GetRecipesByDifficulty(int Difficolta, string orderby)
        {
            return await _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE ricetta.Difficolta = {Difficolta}" +
                $"ORDER BY {orderby}"
            );
        }

        public async Task<List<Ricetta>> GetRecipesByTime(int Tempo, string orderby)
        {
            return await _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE ricetta.Tempo = {Tempo}" +
                $"ORDER BY {orderby}"
            ); ;
        }

        public async Task<List<Ricetta>> GetRecipeById(int IdRicetta)
        {
            return await _dbService.GetData<Ricetta>(
                $"SELECT ricetta.*\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE ricetta.IdRicetta = {IdRicetta};"
            );
        }

        public async Task<List<Collezione>> GetCollectionById(int IdCollezione)
        {
            return await _dbService.GetData<Collezione>(
                $"SELECT collezione.*, categoria_nutrizionale.Nome AS NomeCategoria\r\n" +
                $"FROM collezione\r\n" +
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria=collezione.IdCategoria\r\n" +
                $"WHERE IdCollezione = {IdCollezione};"
            );
        }

        public async Task<List<Collezione>> GetCollections()
        {
            return await _dbService.GetData<Collezione>(
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

        public async Task<List<Collezione>> GetDiets()
        {
            return await _dbService.GetData<Collezione>(
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

        public async Task<List<Utente>> GetUserById(int id)
        {
            return await _dbService.GetData<Utente>(
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

        public async Task<List<Ricetta>> GetMonthRecipes()
        {
            return await _dbService.GetData<Ricetta>(
                $"DROP VIEW IF EXISTS r1;\r\n" +
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
                $"LIMIT 3;"
            );
        }

        public async Task<List<Ricetta>> GetSearchedRecipes(string text, string orderby)
        {
            return await _dbService.GetData<Ricetta>(
                $"DROP VIEW IF EXISTS r1;\r\n" +
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
                $"ORDER BY {orderby}\r\n" +
                $"LIMIT 10;"
            );
        }

        public async Task<List<Collezione>> GetSearchedCollections(int dieta, string text)
        {
            return await _dbService.GetData<Collezione>(
                $"DROP VIEW IF EXISTS c1_temp;\r\n" +
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
                $"WHERE c1_temp.nome LIKE \"%{text}%\" OR categoria_nutrizionale.Nome LIKE \"%{text}%\" OR ricetta.Nome LIKE \"%{text}%\";"
            );
        }

        public async Task<List<CategoriaNutrizionale>> GetNutritionalCategories()
        {
            return await _dbService.GetData<CategoriaNutrizionale>(
                $"SELECT categoria_nutrizionale.*\r\n" +
                $"FROM categoria_nutrizionale"
            );
        }

        public async Task<List<CategoriaNutrizionale>> GetNutritionalCategories(int IdRicetta)
        {
            return await _dbService.GetData<CategoriaNutrizionale>(
                $"SELECT DISTINCT categoria_nutrizionale.*\r\n" +
                $"FROM categoria_nutrizionale\r\n" +
                $"JOIN ingrediente ON categoria_nutrizionale.IdCategoria = ingrediente.IdCategoria\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"WHERE ingrediente_ricetta.IdRicetta = {IdRicetta};"
            );
        }

        public async Task<List<VotiRicetta>> GetRecipeRatingsCountGroupByVoto(int IdRicetta)
        {
            return await _dbService.GetData<VotiRicetta>(
                $"SELECT IFNULL(SUM(CASE WHEN Voto = 1 THEN 1 ELSE 0 END), 0) AS VotiPositivi, IFNULL(SUM(CASE WHEN Voto = 0 THEN 1 ELSE 0 END), 0) AS VotiNegativi\r\n" +
                $"FROM valutazione_ricetta\r\n" +
                $"WHERE IdRicetta = {IdRicetta};"
            );
        }

        public async Task<List<Valutazione>> GetRecipeRatingsByRecipe(int IdRicetta)
        {
            return await _dbService.GetData<Valutazione>(
                $"SELECT valutazione_ricetta.IdUtente, IdRicetta, Voto, DataValutazione, Commento, utente.Username AS NomeUtente, utente.Foto AS FotoUtente\r\n" +
                $"FROM valutazione_ricetta\r\n" +
                $"JOIN utente ON utente.IdUtente=valutazione_ricetta.IdUtente\r\n" +
                $"WHERE IdRicetta = {IdRicetta}\r\n" +
                $"ORDER BY DataValutazione DESC;"
            );
        }

        public async Task InsertRecipeRating(List<Tuple<string, object>> valutazione)
        {
            if (!await _dbService.ExistInTable($"SELECT * FROM valutazione_ricetta WHERE IdUtente = {valutazione[0].Item2} AND IdRicetta = {valutazione[1].Item2} AND Voto = {valutazione[2].Item2};"))
            {
                if (await _dbService.ExistInTable($"SELECT * FROM valutazione_ricetta WHERE IdUtente = {valutazione[0].Item2} AND IdRicetta = {valutazione[1].Item2} AND Voto != {valutazione[2].Item2};"))
                {
                    await _dbService.RemoveElement($"DELETE FROM valutazione_ricetta WHERE IdUtente = {valutazione[0].Item2} AND IdRicetta = {valutazione[1].Item2};");
                }
                await _dbService.InsertElement(
                    valutazione,
                    $@"INSERT INTO valutazione_ricetta (IdUtente, IdRicetta, Voto, DataValutazione, Commento) VALUES (?, ?, ?, NOW(), '');"
                );
            }
        }

        public async Task<List<Ricetta>> GetFilteredRecipes(string difficulty,string time, string orderby, string category)
        {
            string query =
                $"WITH v1(IdRicetta,Nome,Descrizione,Passaggi,Foto,Difficolta,Tempo,DataCreazione,IdUtente,NumeroLike) AS(\r\n" +
                $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                $"FROM ricetta\r\n" +
                $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n";
            if (difficulty != null || time != null)
            {
                query += " WHERE ";
            }
            if (difficulty != null)
            {
                query += $"ricetta.Difficolta =  {difficulty}";
                if (time != null)
                {
                    query += " AND ";
                }
            }
            if (time != null)
            {
                query += $" FLOOR(ricetta.Tempo / 10) * 10 = FLOOR({time} / 10) * 10 ";
            }
            //end of view
            query += "\r\nGROUP BY ricetta.IdRicetta)\r\n" + "SELECT DISTINCT v1.*\r\n" + "FROM v1\r\n";

            if (category != null)
            {
                query +=
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = v1.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente.IdIngrediente = ingrediente_ricetta.IdIngrediente\r\n" +
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = ingrediente.IdCategoria\r\n" +
                $"WHERE categoria_nutrizionale.Nome IN (\'{category}\')\r\n";
            }
            
            query += $"ORDER BY {orderby};";
            return await dbService.GetData<Ricetta>(query);                     
        }
    }
}
