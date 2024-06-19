using Daidokoro.Model;

namespace Daidokoro.ViewModel
{
    public interface IMainViewModel
    {
        DBService dbService { get; }

        bool InitDBSettings(DBCredentials dbs);

        List<Ingrediente> GetIngredients(int IdRicetta);
        List<Ricetta> GetRecipes();
        List<Ricetta> GetRecipesByCollection(int IdCollezione);
        List<Ricetta> GetRecipesByDifficulty(int Difficolta);
        List<Ricetta> GetRecipesByTime(int Tempo);
        Ricetta GetRecipeById(int IdRicetta);
        List<Collezione> GetCollections();
        Collezione GetCollectionById(int IdCollezione);
        List<Collezione> GetDiets();
        List<Utente> GetUserById(int id);
        List<Ricetta> GetMonthRecipes();
        List<Ricetta> GetSearchedRecipes(string text);
        List<Collezione> GetSearchedCollections(int dieta, string text);
        List<CategoriaNutrizionale> GetNutritionalCategory(int IdRicetta);
    }
}
