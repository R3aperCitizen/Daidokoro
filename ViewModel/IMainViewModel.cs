﻿using Daidokoro.Model;

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
        Task<List<Ricetta>> GetRecipesByDifficulty(int Difficolta,string orderby);
        Task<List<Ricetta>> GetRecipesByTime(int Tempo,string orderby);
        Task<List<Ricetta>> GetRecipesByUser(int userID);
        Task<List<Ricetta>> GetLikedRecipes(int userID);
        Task<List<Ricetta>> GetReviewedRecipes(int userID);
        Task<List<Ricetta>> GetRecipeById(int IdRicetta);
        Task<List<Collezione>> GetCollections();
        Task<List<Collezione>> GetCollectionById(int IdCollezione);
        Task<List<Collezione>> GetDiets();
        Task<List<Utente>> GetUserById(int id);
        Task<List<Ricetta>> GetMonthRecipes();
        Task<List<Ricetta>> GetSearchedRecipes(string text, string orderby);
        Task<List<Ricetta>> GetSearchedRecipes(string Name, int IdCategoria);
        Task<List<Ricetta>> GetFilteredRecipes(string difficulty, string time, string orderby, string categories, string searchBarText);
        Task<List<Ingrediente>> GetSearchedIngredients(string text);
        Task<List<CategoriaNutrizionale>> GetNutritionalCategories();
        Task<List<CategoriaNutrizionale>> GetNutritionalCategories(int IdRicetta);
        Task<List<VotiRicetta>> GetRatingsCountGroupByVoto(int Id, bool isRecipe);
        Task<List<Valutazione>> GetRatingsById(int Id, bool isRecipe);
        Task InsertRating(List<Tuple<string, object>> valutazione, bool isRecipe);
        Task<bool> InsertReviewIfRatedByUser(int Id, string Commento, bool isRecipe);
        Task InsertNewRecipe(List<Tuple<string, object>> recipe);
        Task InsertRecipeIngredient(List<Tuple<string, object>> ingredientRecipe);
        Task<int> GetInsertedRecipeId();
        Task InsertNewCollection(List<Tuple<string, object>> collection);
        Task InsertCollectionRecipe(List<Tuple<string, object>> recipeCollection);
        Task<int> GetInsertedCollectionId();
        Task<bool> CanUserLogin(string Email, string Password);
        Task<string> GetLoggedUserId(string Email, string Password);
        Task AddOrRemoveRecipeFromLiked(int IdRicetta);
        public Task<List<Collezione>> getFilteredDiets(string text, string difficolta, string Data, string ordinamento, string Nricette,int dieta);

        public static Dictionary<string, string> sortings = new Dictionary<string, string>()
        {
            {"Difficolta","Difficolta" },
            {"Tempo","Tempo" },
            {"Like","NumeroLike" }
        };

        public static Dictionary<string, string> DietSort = new Dictionary<string, string>()
        {
            { "DifficoltaMedia", "avDiff" },
            { "NumeroRicette", "num" },
            {"Data","DataCreazione" }
        };
    }
}
