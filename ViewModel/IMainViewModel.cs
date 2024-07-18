using Daidokoro.Model;

namespace Daidokoro.ViewModel
{
    public interface IMainViewModel
    {
        DBService dbService { get; }

        bool InitDBSettings(DBCredentials dbs);

        Task<T> LoadAndGet<T>(Task<T> task);
        Task Load(Task task);

        Task<int> GetCurrentUserId();
        Task<List<Ingrediente>> GetIngredients(int IdRicetta);
        Task<List<Ricetta>> GetRecipes();
        Task<List<Ricetta>> GetRecipesByCollection(int IdCollezione);
        Task<List<Ricetta>> GetRecipesByDifficulty(int difficulty, string orderby);
        Task<List<Ricetta>> GetRecipesByTime(int time, string orderby);
        Task<List<Ricetta>> GetRecipesByUser(int userID);
        Task<List<Ricetta>> GetLikedRecipes(int userID);
        Task<List<Ricetta>> GetReviewedRecipes(int userID);
        Task<List<Ricetta>> GetRecipeById(int IdRicetta);
        Task<List<Collezione>> GetCollectionsOrDiets(int Dieta);
        Task<List<Collezione>> GetCollectionById(int IdCollezione);
        Task<List<Collezione>> GetCollectionsByUser(int userID);
        Task<List<Utente>> GetUserById(int id);
        Task<List<Ricetta>> GetMonthRecipes();
        Task<List<Ricetta>> GetSearchedRecipes(string name, int IdCategoria);
        Task<List<Ricetta>> GetFilteredRecipes(string difficulty, string time, string orderby, string categories, string searchBarText);
        Task<List<Ingrediente>> GetSearchedIngredients(string text);
        Task<List<CategoriaNutrizionale>> GetNutritionalCategories();
        Task<List<CategoriaNutrizionale>> GetNutritionalCategories(int IdRicetta);
        Task<List<CategoriaNutrizionale>> GetUnlockedNutritionalCategories();
        Task<List<VotiRicetta>> GetRatingsCountGroupByVoto(int id, bool isRecipe);
        Task<List<Valutazione>> GetRatingsById(int id, bool isRecipe);
        Task InsertRating(List<Tuple<string, object>> rating, bool isRecipe);
        Task<bool> InsertReviewIfRatedByUser(int id, string comment, bool isRecipe);
        Task InsertNewRecipe(List<Tuple<string, object>> recipe);
        Task InsertRecipeIngredient(List<Tuple<string, object>> ingredientRecipe);
        Task<int> GetInsertedRecipeId();
        Task InsertNewCollection(List<Tuple<string, object>> collection);
        Task InsertCollectionRecipe(List<Tuple<string, object>> recipeCollection);
        Task<int> GetInsertedCollectionId();
        Task<bool> CanUserLogin(string email, string password);
        Task<string> GetLoggedUserId(string email);
        Task<bool> CanUserRegister(string email);
        Task RegisterUser(List<Tuple<string, object>> userData);
        Task GiveRegisterObjective();
        Task<bool> AddOrRemoveRecipeFromLiked(int IdRicetta);
        Task<bool> IsRecipeLikedByUser(int IdRicetta);
        public Task<List<Collezione>> GetFilteredCollections(string text, string difficulty, string date, string orderby, string recipeNumber, int dieta, string nutritionalCategory);

        public static Dictionary<string, string> RecipeSortings = new()
        {
            { "Like ▲", "NumeroLike DESC" },
            { "Like ▼", "NumeroLike" },
            { "Difficolta ▲", "Difficolta DESC" },
            { "Difficolta ▼", "Difficolta" },
            { "Tempo ▲", "Tempo DESC" },
            { "Tempo ▼", "Tempo" }
        };

        public static Dictionary<string, string> DietSortings = new()
        {
            { "Difficolta Media ▲", "avDiff DESC" },
            { "Difficolta Media ▼", "avDiff" },
            { "Numero Ricette ▲", "num DESC" },
            { "Numero Ricette ▼", "num" },
            { "Data Creazione ▲", "DataCreazione DESC" },
            { "Data Creazione ▼", "DataCreazione" }
        };
    }
}
