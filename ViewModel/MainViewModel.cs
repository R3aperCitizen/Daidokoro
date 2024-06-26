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

        public async Task<List<Ricetta>> GetRecipesByDifficulty(int Difficolta, string orderby)
        {
            return await _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE ricetta.Difficolta = {Difficolta}\r\n" +
                $"ORDER BY {orderby}"
            );
        }

        public async Task<List<Ricetta>> GetRecipesByTime(int Tempo, string orderby)
        {
            return await _dbService.GetData<Ricetta>(
                $"SELECT *\r\n" +
                $"FROM ricetta\r\n" +
                $"WHERE ricetta.Tempo = {Tempo}\r\n" +
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

        public async Task<List<Collezione>> GetSearchedCollections(int dieta, string text)
        {
            return await _dbService.GetData<Collezione>(
                $"WITH v1(IdCollezione,Nome,Descrizione,Dieta,DataCreazione,IdUtente,IdCategoria,NomeCategoria,FotoRicetta) AS(\r\n" +
                $"SELECT c1.*, categoria_nutrizionale.Nome AS NomeCategoria, ricetta.Foto AS FotoRicetta\r\n" +
                $"FROM collezione AS c1\r\n" +
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = c1.IdCategoria\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdCollezione = c1.IdCollezione\r\n" +
                $"JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta\r\n" +
                $"WHERE Dieta = {dieta} AND ricetta.IdRicetta = (\r\n" +
                $"SELECT MIN(ricetta.IdRicetta)\r\n" +
                $"FROM ricetta\r\n" +
                $"JOIN ricetta_collezione ON ricetta_collezione.IdRicetta = ricetta.IdRicetta\r\n" +
                $"WHERE ricetta_collezione.IdCollezione = c1.IdCollezione)\r\n" +
                $"SELECT DISTINCT v1.*\r\n" +
                $"FROM v1\r\n" +
                $"JOIN categoria_nutrizionale ON categoria_nutrizionale.IdCategoria = v1.IdCategoria\r\n" +
                $"JOIN ricetta_collezione ON v1.IdCollezione = ricetta_collezione.IdCollezione\r\n" +
                $"JOIN ricetta ON ricetta.IdRicetta = ricetta_collezione.IdRicetta\r\n" +
                $"WHERE v1.nome LIKE \"%{text}%\" OR categoria_nutrizionale.Nome LIKE \"%{text}%\" OR ricetta.Nome LIKE \"%{text}%\";"
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

        public async Task<List<VotiRicetta>> GetRatingsCountGroupByVoto(int Id, bool isRecipe)
        {
            string table = isRecipe ? "valutazione_ricetta" : "valutazione_collezione";
            string idName = isRecipe ? "IdRicetta" : "IdCollezione";
            return await _dbService.GetData<VotiRicetta>(
                $"SELECT IFNULL(SUM(CASE WHEN Voto = 1 THEN 1 ELSE 0 END), 0) AS VotiPositivi, IFNULL(SUM(CASE WHEN Voto = 0 THEN 1 ELSE 0 END), 0) AS VotiNegativi\r\n" +
                $"FROM {table}\r\n" +
                $"WHERE {idName} = {Id};"
            );
        }

        public async Task<List<Valutazione>> GetRatingsById(int Id, bool isRecipe)
        {
            string table = isRecipe ? "valutazione_ricetta" : "valutazione_collezione";
            string idName = isRecipe ? "IdRicetta" : "IdCollezione";
            return await _dbService.GetData<Valutazione>(
                $"SELECT {table}.IdUtente, {idName}, Voto, DataValutazione, Commento, utente.Username AS NomeUtente, utente.Foto AS FotoUtente\r\n" +
                $"FROM {table}\r\n" +
                $"JOIN utente ON utente.IdUtente={table}.IdUtente\r\n" +
                $"WHERE {idName} = {Id}\r\n" +
                $"ORDER BY DataValutazione DESC;"
            );
        }

        public async Task InsertRating(List<Tuple<string, object>> valutazione, bool isRecipe)
        {
            string table = isRecipe ? "valutazione_ricetta" : "valutazione_collezione";
            string idName = isRecipe ? "IdRicetta" : "IdCollezione";
            if (int.TryParse(await SecureStorage.Default.GetAsync("IdUtente"), out int IdUtente))
            {
                var v = valutazione;
                v.Insert(0, new("IdUtente", IdUtente));
                if (!await _dbService.ExistInTable($"SELECT * FROM {table} WHERE IdUtente = {valutazione[0].Item2} AND {idName} = {valutazione[1].Item2} AND Voto = {valutazione[2].Item2};"))
                {
                    if (await _dbService.ExistInTable($"SELECT * FROM {table} WHERE IdUtente = {valutazione[0].Item2} AND {idName} = {valutazione[1].Item2} AND Voto != {valutazione[2].Item2};"))
                    {
                        await _dbService.RemoveOrUpdateElement($"DELETE FROM {table} WHERE IdUtente = {valutazione[0].Item2} AND {idName} = {valutazione[1].Item2};");
                    }
                    await _dbService.InsertElement(
                        v,
                        $@"INSERT INTO {table} (IdUtente, {idName}, Voto, DataValutazione, Commento) VALUES (?, ?, ?, NOW(), '');"
                    );
                }
            }
        }

        public async Task<bool> InsertReviewIfRatedByUser(int Id, string Commento, bool isRecipe)
        {
            string table = isRecipe ? "valutazione_ricetta" : "valutazione_collezione";
            string idName = isRecipe ? "IdRicetta" : "IdCollezione";
            if (int.TryParse(await SecureStorage.Default.GetAsync("IdUtente"), out int IdUtente))
            {
                if (await _dbService.ExistInTable($"SELECT * FROM {table} WHERE IdUtente = {IdUtente} AND {idName} = {Id};"))
                {
                    await _dbService.RemoveOrUpdateElement(
                        $"UPDATE {table}\r\n" +
                        $"SET Commento = \'{Commento}\'\r\n" +
                        $"WHERE IdUtente = {IdUtente} AND {idName} = {Id};"
                    );
                    return true;
                }
            }
            return false;
        }

        public async Task<List<Ricetta>> GetFilteredRecipes(string difficulty,string time, string orderby, string category,string text)
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

        public async Task<bool> CanUserLogin(string Email, string Password)
        {
            return await _dbService.ExistInTable($"SELECT * FROM utente WHERE Email = \'{Email}\' AND Pwd = \'{Password}\'");
        }

        public async Task<string> GetLoggedUserId(string Email, string Password)
        {
            return (await _dbService.GetData<Utente>($"SELECT * FROM utente WHERE Email = \'{Email}\' AND Pwd = \'{Password}\'"))[0].IdUtente.ToString();
        }

        public async Task<List<Collezione>> getFilteredDiets(string text, string difficolta, string Data, string ordinamento,string Nricette)
        {
            if (text != null)
            {
                /* use daidokoro;
                with v2 as (
                With v1 AS(
                Select *
                from collezione
                where collezione.Dieta = 1 && collezione.DataCreazione = 20240603)
                Select v1.*,avg(ricetta.difficolta) as avDiff, count(ricetta.difficolta) as num
                from v1
                join ricetta_collezione on ricetta_collezione.IdCollezione = v1.IdCollezione
                join ricetta on ricetta.IdRicetta = ricetta_collezione.IdRicetta
                group by v1.IdCollezione
                order by num)
                Select v2.*
                from v2
                where v2.num = 2*/
                
            }

            return null;
        }
    }
}
