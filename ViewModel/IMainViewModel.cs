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
        Task<List<VotiRicetta>> GetRecipeRatingsCountGroupByVoto(int IdRicetta);
        Task<List<Valutazione>> GetRecipeRatingsByRecipe(int IdRicetta);
        Task InsertRecipeRating(List<Tuple<string, object>> valutazione);

        public static Dictionary<string, string> sortings = new Dictionary<string, string>()
        {
            {"difficolta","Difficolta" },
            {"tempo","Tempo" },
            {"voti","NumeroVoti" }
        };
        public static Dictionary<string, string> categories = new Dictionary<string, string>()
        {
            {"gluten-free","GlutenFree" },
            {"cascaPalle" , "cascaPalle"}
        };
    }
}
