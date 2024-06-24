using Daidokoro.Model;

namespace Daidokoro.ViewModel
{
    public interface IMainViewModel
    {
        DBService dbService { get; }

        bool InitDBSettings(DBCredentials dbs);

        Task<List<Ingrediente>> GetIngredients(int IdRicetta);
        Task<List<Ricetta>> GetRecipes();
        Task<List<Ricetta>> GetRecipesByCollection(int IdCollezione);
        Task<List<Ricetta>> GetRecipesByDifficulty(int Difficolta,string orderby);
        Task<List<Ricetta>> GetRecipesByTime(int Tempo,string orderby);
        Task<List<Ricetta>> GetRecipeById(int IdRicetta);
        Task<List<Collezione>> GetCollections();
        Task<List<Collezione>> GetCollectionById(int IdCollezione);
        Task<List<Collezione>> GetDiets();
        Task<List<Utente>> GetUserById(int id);
        Task<List<Ricetta>> GetMonthRecipes();
        Task<List<Ricetta>> GetSearchedRecipes(string text,string orderby);
        Task<List<Ricetta>> GetFilteredRecipes(string difficulty, string time, string orderby, string categories);
        Task<List<Collezione>> GetSearchedCollections(int dieta, string text);
        Task<List<CategoriaNutrizionale>> GetNutritionalCategories();
        Task<List<CategoriaNutrizionale>> GetNutritionalCategories(int IdRicetta);
        Task<List<VotiRicetta>> GetRatingsCountGroupByVoto(int Id, bool isRecipe);
        Task<List<Valutazione>> GetRatingsById(int Id, bool isRecipe);
        Task InsertRating(List<Tuple<string, object>> valutazione, bool isRecipe);
        Task<bool> InsertReviewIfRatedByUser(int Id, string Commento, bool isRecipe);
        Task<bool> CanUserLogin(string Email, string Password);
        Task<string> GetLoggedUserId(string Email, string Password);

        public static Dictionary<string, string> sortings = new Dictionary<string, string>()
        {
            {"Difficolta","Difficolta" },
            {"Tempo","Tempo" },
            {"Like","NumeroLike" }
        };
        public static Dictionary<string, string> categories = new Dictionary<string, string>()
        {
            {"gluten-free", "Gluten-Free"},
            {"cascaPalle" , "cascaPalle"}
        };
    }
}
