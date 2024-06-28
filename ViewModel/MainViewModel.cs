using CommunityToolkit.Maui.Views;
using Daidokoro.Model;
using Daidokoro.View.Controls;
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

        public async Task<T> LoadAndGet<T>(Task<T> task)
        {
            var loadingPopup = new LoadingPopup();
            Application.Current.MainPage.ShowPopup(loadingPopup);
            await task;
            loadingPopup.Close();
            return task.Result;
        }

        public async Task Load(Task task)
        {
            var loadingPopup = new LoadingPopup();
            Application.Current.MainPage.ShowPopup(loadingPopup);
            await task;
            loadingPopup.Close();
        }

        public async Task<int> GetCurrentUserId()
        {
            string value = await SecureStorage.Default.GetAsync("IdUtente");
            return int.Parse(value);
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
                $"WITH v1(IdRicetta,Nome,Descrizione,Passaggi,Foto,Difficolta,Tempo,DataCreazione,IdUtente,NumeroLike) AS(\r\n" +
                $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                $"FROM ricetta\r\n" +
                $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n" +
                $"GROUP BY ricetta.IdRicetta\r\n" +
                $"LIMIT 10)\r\n" +
                $"SELECT DISTINCT v1.*\r\n" +
                $"FROM v1\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = v1.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"ORDER BY NumeroLike DESC;"
           );
        }

        public async Task<List<Ricetta>> GetRecipesByCollection(int IdCollezione)
        {
            return await _dbService.GetData<Ricetta>(
                $"WITH v1(IdRicetta,Nome,Descrizione,Passaggi,Foto,Difficolta,Tempo,DataCreazione,IdUtente,NumeroLike) AS(\r\n" +
                $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                $"FROM ricetta\r\n" +
                $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdRicetta=ricetta.IdRicetta\r\n" +
                $"WHERE ricetta_collezione.IdCollezione = {IdCollezione}\r\n" +
                $"GROUP BY ricetta.IdRicetta)\r\n" +
                $"SELECT DISTINCT v1.*\r\n" +
                $"FROM v1\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = v1.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"ORDER BY NumeroLike DESC;"
            );
        }

        public async Task<List<Ricetta>> GetRecipesByDifficulty(int difficulty, string orderby)
        {
            return await _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE ricetta.Difficolta = {difficulty}\r\n" +
                $"ORDER BY {orderby}"
            );
        }

        public async Task<List<Ricetta>> GetRecipesByTime(int time, string orderby)
        {
            return await _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE ricetta.Tempo = {time}\r\n" +
                $"ORDER BY {orderby}\r\n"
            );
        }

        public async Task<List<Ricetta>> GetRecipesByUser(int userID)
        {
            return await _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE ricetta.IdUtente = {userID}\r\n"
            );
        }

        public async Task<List<Ricetta>> GetLikedRecipes(int userID)
        {
            return await _dbService.GetData<Ricetta>(
                $"SELECT ricetta.*\r\n" +
                $"FROM ricetta\r\n" +
                $"JOIN likes ON ricetta.IdRicetta = likes.IdRicetta\r\n" +
                $"WHERE likes.IdUtente = {userID}\r\n"
            );
        }

        public async Task<List<Ricetta>> GetReviewedRecipes(int userID)
        {
            return await _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"JOIN valutazione_ricetta ON ricetta.IdRicetta = valutazione_ricetta.IdRicetta\r\n" +
                $"WHERE valutazione_ricetta.IdUtente = {userID}\r\n"
            );
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

        public async Task<List<Collezione>> GetCollectionsOrDiets(int Dieta)
        {
            return await _dbService.GetData<Collezione>(
                $"SELECT c1.*, categoria_nutrizionale.Nome AS NomeCategoria, ricetta.Foto AS FotoRicetta\r\n" +
                $"FROM collezione AS c1\r\n" +
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = c1.IdCategoria\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdCollezione = c1.IdCollezione\r\n" +
                $"JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta\r\n" +
                $"WHERE Dieta = {Dieta} AND ricetta.IdRicetta = (\r\n" +
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
                $"LEFT JOIN (SELECT utente.IdUtente, COUNT(*) AS ReviewCount FROM utente JOIN valutazione_ricetta ON valutazione_ricetta.IdUtente = utente.IdUtente GROUP BY valutazione_ricetta.IdUtente) AS temp_reviews\r\n" +
                $"ON utente.IdUtente = temp_reviews.IdUtente\r\n" +
                $"LEFT JOIN (SELECT utente.IdUtente, COUNT(*) AS RecipeCount FROM utente JOIN ricetta ON ricetta.IdUtente = utente.IdUtente GROUP BY ricetta.IdUtente) AS temp_recipes\r\n" +
                $"ON utente.IdUtente = temp_recipes.IdUtente\r\n" +
                $"WHERE utente.IdUtente = {id};"
            );
        }

        public async Task<List<Ricetta>> GetMonthRecipes()
        {
            return await _dbService.GetData<Ricetta>(
                $"WITH v1(IdRicetta,Nome,Descrizione,Passaggi,Foto,Difficolta,Tempo,DataCreazione,IdUtente,NumeroLike) AS(\r\n" +             
                $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                $"FROM ricetta\r\n" +
                $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n" +
                $"GROUP BY ricetta.IdRicetta)\r\n" +
                $"SELECT DISTINCT v1.*\r\n" +
                $"FROM v1\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = v1.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"ORDER BY NumeroLike DESC\r\n" +
                $"LIMIT 3;"
            );
        }

        public async Task<List<Ricetta>> GetSearchedRecipes(string text, string orderby)
        {
            return await _dbService.GetData<Ricetta>(
                $"WITH v1(IdRicetta,Nome,Descrizione,Passaggi,Foto,Difficolta,Tempo,DataCreazione,IdUtente,NumeroLike) AS(\r\n" +               
                $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                $"FROM ricetta\r\n" +
                $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n" +
                $"GROUP BY ricetta.IdRicetta)\r\n" +
                $"SELECT DISTINCT v1.*\r\n" +
                $"FROM v1\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = v1.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                $"JOIN categoria_nutrizionale ON ingrediente.IdCategoria=categoria_nutrizionale.IdCategoria\r\n" + 
                $"WHERE LOWER(categoria_nutrizionale.Nome) LIKE \"%{text}%\"\r\n" +
                $"OR LOWER(ingrediente.Nome) LIKE \"%{text}%\"\r\n" +
                $"OR LOWER(v1.Nome) LIKE \"%{text}%\"\r\n" +
                $"ORDER BY {orderby}\r\n" +
                $"LIMIT 10;"
            );
        }
        public async Task<List<Ricetta>> GetSearchedRecipes(string name, int IdCategoria)
        {
            return await _dbService.GetData<Ricetta>(
                $"WITH v1(IdRicetta,Nome,Descrizione,Passaggi,Foto,Difficolta,Tempo,DataCreazione,IdUtente,NumeroLike) AS(\r\n" +
                $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                $"FROM ricetta\r\n" +
                $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n" +
                $"GROUP BY ricetta.IdRicetta)\r\n" +
                $"SELECT DISTINCT v1.*\r\n" +
                $"FROM v1\r\n" +
                $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta=v1.IdRicetta\r\n" +
                $"JOIN ingrediente ON ingrediente.IdIngrediente=ingrediente_ricetta.IdIngrediente\r\n" +
                $"WHERE v1.Nome LIKE \"%{name}%\"\r\n" +
                $"AND ingrediente.IdIngrediente IN (\r\n" +
                $"SELECT IdIngrediente FROM ingrediente WHERE IdCategoria = {IdCategoria});"
            );
        }

        public async Task<List<Ingrediente>> GetSearchedIngredients(string text)
        {
            return await _dbService.GetData<Ingrediente>(
                $"SELECT *\r\n" +
                $"FROM ingrediente\r\n" +
                $"WHERE LOWER(Nome) LIKE \"%{text}%\";"
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

        public async Task<List<VotiRicetta>> GetRatingsCountGroupByVoto(int id, bool isRecipe)
        {
            string table = isRecipe ? "valutazione_ricetta" : "valutazione_collezione";
            string idName = isRecipe ? "IdRicetta" : "IdCollezione";
            return await _dbService.GetData<VotiRicetta>(
                $"SELECT IFNULL(SUM(CASE WHEN Voto = 1 THEN 1 ELSE 0 END), 0) AS VotiPositivi, IFNULL(SUM(CASE WHEN Voto = 0 THEN 1 ELSE 0 END), 0) AS VotiNegativi\r\n" +
                $"FROM {table}\r\n" +
                $"WHERE {idName} = {id};"
            );
        }

        public async Task<List<Valutazione>> GetRatingsById(int id, bool isRecipe)
        {
            string table = isRecipe ? "valutazione_ricetta" : "valutazione_collezione";
            string idName = isRecipe ? "IdRicetta" : "IdCollezione";
            return await _dbService.GetData<Valutazione>(
                $"SELECT {table}.IdUtente, {idName}, Voto, DataValutazione, Commento, utente.Username AS NomeUtente, utente.Foto AS FotoUtente\r\n" +
                $"FROM {table}\r\n" +
                $"JOIN utente ON utente.IdUtente={table}.IdUtente\r\n" +
                $"WHERE {idName} = {id}\r\n" +
                $"ORDER BY DataValutazione DESC;"
            );
        }

        public async Task InsertRating(List<Tuple<string, object>> rating, bool isRecipe)
        {
            string table = isRecipe ? "valutazione_ricetta" : "valutazione_collezione";
            string idName = isRecipe ? "IdRicetta" : "IdCollezione";
            if (int.TryParse(await SecureStorage.Default.GetAsync("IdUtente"), out int IdUtente))
            {
                var r = rating;
                r.Insert(0, new("IdUtente", IdUtente));
                if (!await _dbService.ExistInTable($"SELECT * FROM {table} WHERE IdUtente = {rating[0].Item2} AND {idName} = {rating[1].Item2} AND Voto = {rating[2].Item2};"))
                {
                    if (await _dbService.ExistInTable($"SELECT * FROM {table} WHERE IdUtente = {rating[0].Item2} AND {idName} = {rating[1].Item2} AND Voto != {rating[2].Item2};"))
                    {
                        await _dbService.RemoveOrUpdateElement($"DELETE FROM {table} WHERE IdUtente = {rating[0].Item2} AND {idName} = {rating[1].Item2};");
                    }
                    await _dbService.InsertElement(
                        r,
                        $@"INSERT INTO {table} (IdUtente, {idName}, Voto, DataValutazione, Commento) VALUES (?, ?, ?, NOW(), '');"
                    );
                }
            }
        }

        public async Task<bool> InsertReviewIfRatedByUser(int id, string comment, bool isRecipe)
        {
            string table = isRecipe ? "valutazione_ricetta" : "valutazione_collezione";
            string idName = isRecipe ? "IdRicetta" : "IdCollezione";
            if (int.TryParse(await SecureStorage.Default.GetAsync("IdUtente"), out int IdUtente))
            {
                if (await _dbService.ExistInTable($"SELECT * FROM {table} WHERE IdUtente = {IdUtente} AND {idName} = {id};"))
                {
                    await _dbService.RemoveOrUpdateElement(
                        $"UPDATE {table}\r\n" +
                        $"SET Commento = \'{comment}\'\r\n" +
                        $"WHERE IdUtente = {IdUtente} AND {idName} = {id};"
                    );
                    return true;
                }
            }
            return false;
        }

        public async Task<List<Ricetta>> GetFilteredRecipes(string difficulty, string time, string orderby, string category, string text)
        {
            // the query generator is structured in 2 principal tables, v2 and v1, v2 contains other tables
            // or no tables, is necessary so that v1 can reference generally to whatever thing is in v2
            // and after v1 there is the last filter and data retrieval

            string query =
                $"WITH v1(IdRicetta,Nome,Descrizione,Passaggi,Foto,Difficolta,Tempo,DataCreazione,IdUtente,NumeroLike) AS(\r\n" +
                $"WITH v2(IdRicetta,Nome,Descrizione,Passaggi,Foto,Difficolta,Tempo,DataCreazione,IdUtente,NumeroLike) AS(\r\n" +
                $"WITH v3(IdRicetta,Nome,Descrizione,Passaggi,Foto,Difficolta,Tempo,DataCreazione,IdUtente,NumeroLike) AS(\r\n";
            if (text != null && text != string.Empty)
            {
                query +=                
                    $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                    $"FROM ricetta\r\n" +
                    $"LEFT JOIN likes ON likes.IdRicetta = ricetta.IdRicetta\r\n" +
                    $"GROUP BY ricetta.IdRicetta)\r\n" +
                    $"SELECT DISTINCT v3.*\r\n" +
                    $"FROM v3\r\n" +
                    $"JOIN ingrediente_ricetta ON ingrediente_ricetta.IdRicetta = v3.IdRicetta\r\n" +
                    $"JOIN ingrediente ON ingrediente_ricetta.IdIngrediente = ingrediente.IdIngrediente\r\n" +
                    $"JOIN categoria_nutrizionale ON ingrediente.IdCategoria=categoria_nutrizionale.IdCategoria\r\n" +
                    $"WHERE LOWER(categoria_nutrizionale.Nome) LIKE \"%{text}%\"\r\n" +
                    $"OR LOWER(ingrediente.Nome) LIKE \"%{text}%\"\r\n" +
                    $"OR LOWER(v3.Nome) LIKE \"%{text}%\"\r\n" +
                    $"LIMIT 10)\r\n";
            }
            else
            {
                query +=
                    $"SELECT ricetta.*, COUNT(likes.IdRicetta) AS NumeroLike\r\n" +
                    $"FROM ricetta)\r\n" +
                    $"SELECT DISTINCT v3.*\r\n" +
                    $"FROM v3)\r\n";
            }
            query +=
                $"SELECT v2.*\r\n" +
                $"FROM v2\r\n";             
            if (difficulty != null || time != null)
            {
                query += " WHERE ";
            }
            if (difficulty != null)
            {
                query += $"v2.Difficolta =  {difficulty} ";
                if (time != null)
                {
                    query += " AND ";
                }
            }
            if (time != null)
            {
                query += $" FLOOR(v2.Tempo / 10) * 10 = FLOOR({time} / 10) * 10 ";
            }
            //end of view
            query += "\r\nGROUP BY v2.IdRicetta)\r\n" + "SELECT DISTINCT v1.*\r\n" + "FROM v1\r\n";

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

        public async Task InsertNewRecipe(List<Tuple<string, object>> recipe)
        {
            if (int.TryParse(await SecureStorage.Default.GetAsync("IdUtente"), out int IdUtente))
            {
                var r = recipe;
                r.Add(new("IdUtente", IdUtente));
                await _dbService.InsertElement(
                    r,
                    $@"INSERT INTO ricetta (Nome, Descrizione, Passaggi, Foto, Difficolta, Tempo, DataCreazione, IdUtente) VALUES (?, ?, ?, ?, ?, ?, NOW(), ?);"
                );
            }
        }

        public async Task InsertRecipeIngredient(List<Tuple<string, object>> ingredientRecipe)
        {
            await _dbService.InsertElement(
                ingredientRecipe,
                $@"INSERT INTO ingrediente_ricetta (IdIngrediente, IdRicetta, PesoInGrammi) VALUES (?, ?, ?);"
            );
        }

        public async Task<int> GetInsertedRecipeId()
        {
            return (await _dbService.GetData<Ricetta>(
                    "SELECT MAX(IdRicetta) AS IdRicetta\r\n" +
                    "FROM ricetta"))[0].IdRicetta;
        }

        public async Task InsertNewCollection(List<Tuple<string, object>> collection)
        {
            if (int.TryParse(await SecureStorage.Default.GetAsync("IdUtente"), out int IdUtente))
            {
                var c = collection;
                c.Add(new("IdUtente", IdUtente));
                await _dbService.InsertElement(
                    c,
                    $@"INSERT INTO collezione (Nome, Descrizione, Dieta, DataCreazione, IdUtente, IdCategoria) VALUES (?, ?, ?, NOW(), ?, ?);"
                );
            }
        }

        public async Task InsertCollectionRecipe(List<Tuple<string, object>> recipeCollection)
        {
            await _dbService.InsertElement(
                recipeCollection,
                $@"INSERT INTO ricetta_collezione (IdRicetta, IdCollezione) VALUES (?, ?);"
            );
        }

        public async Task<int> GetInsertedCollectionId()
        {
            return (await _dbService.GetData<Collezione>(
                    "SELECT MAX(IdCollezione) AS IdCollezione\r\n" +
                    "FROM collezione"))[0].IdCollezione;
        }

        public async Task<bool> CanUserLogin(string email, string password)
        {
            return await _dbService.ExistInTable($"SELECT * FROM utente WHERE Email = \'{email}\' AND Pwd = \'{password}\'");
        }

        public async Task<string> GetLoggedUserId(string email, string password)
        {
            return (await _dbService.GetData<Utente>($"SELECT * FROM utente WHERE Email = \'{email}\' AND Pwd = \'{password}\'"))[0].IdUtente.ToString();
        }

        public async Task AddOrRemoveRecipeFromLiked(int IdRicetta)
        {
            if (int.TryParse(await SecureStorage.Default.GetAsync("IdUtente"), out int IdUtente))
            {
                if (await _dbService.ExistInTable(
                    $"SELECT * FROM likes\r\n" +
                    $"WHERE IdRicetta = {IdRicetta} AND IdUtente = {IdUtente};"))
                {
                    await _dbService.RemoveOrUpdateElement(
                        $"DELETE FROM likes\r\n" +
                        $"WHERE IdRicetta = {IdRicetta} AND IdUtente = {IdUtente}"
                    );
                }
                else
                {
                    await _dbService.InsertElement(
                    [
                        new("IdRicetta", IdRicetta),
                        new("IdUtente", IdUtente)
                    ], $"INSERT INTO likes (IdRicetta, IdUtente, Data) VALUES (?, ?, CURDATE());");
                }
            }
        }

        public Task<List<Collezione>> GetFilteredCollections(string text, string difficulty, string date, string orderby, string recipeNumber, int dieta, string nutritionalCategory)
        {
            string query =
            $"WITH v2 AS (\r\n" +
                $"WITH v1 AS (\r\n" +
                    $"SELECT collezione.*, categoria_nutrizionale.Nome as NomeCategoria\r\n" +
                    $"FROM collezione\r\n" +
                    $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = collezione.IdCategoria\r\n" +
                    $"WHERE collezione.Dieta = {dieta}\r\n" +
                    (date == null ? "" : $"AND collezione.DataCreazione = \'{date}\'\r\n") +      
                    (nutritionalCategory == null ? "" : $"AND categoria_nutrizionale.Nome = \'{nutritionalCategory}\'\r\n") +
                $")\r\n" +
                $"SELECT v1.*, avg(ricetta.Difficolta) AS avDiff, COUNT(ricetta.Difficolta) AS num\r\n" +
                $"FROM v1\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdCollezione = v1.IdCollezione\r\n" +
                $"JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta\r\n" +
                (text == null ? "" : $"WHERE LOWER(ricetta.Nome) LIKE \"%{text}%\" OR v1.Nome LIKE \"%{text}%\"\r\n") +
                $"GROUP BY v1.IdCollezione\r\n" +
            $"ORDER BY {orderby})\r\n" +
            $"SELECT v2.*, ricetta.Foto AS FotoRicetta\r\n" +
            $"FROM v2\r\n"+
            $"JOIN ricetta_collezione ON ricetta_collezione.IdCollezione = v2.IdCollezione\r\n"+
            $"JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta\r\n" +
            $"WHERE ricetta_collezione.IdRicetta = ("+
            $"SELECT MIN(ricetta.IdRicetta)\r\n"+
            $"FROM ricetta\r\n"+
            $"JOIN ricetta_collezione ON ricetta_collezione.IdRicetta = ricetta.IdRicetta\r\n"+
            $"WHERE ricetta_collezione.IdCollezione = v2.IdCollezione)\r\n" +
            (recipeNumber == null ? difficulty == null ? "" : $"AND v2.avDiff = {difficulty}" :
            difficulty == null ? $"AND v2.num = {recipeNumber}" : $" AND v2.num = {recipeNumber} AND v2.avDiff = {difficulty}");
            
            return  dbService.GetData<Collezione>(query);
        }
    }
}
