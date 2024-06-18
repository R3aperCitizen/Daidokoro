using Daidokoro.Model;

namespace Daidokoro.ViewModel
{
    public interface IMainViewModel
    {
        DBService dbService { get; }

        bool InitDBSettings(DBCredentials dbs);

        List<Ingrediente> GetIngredienti(int IdRicetta);
        List<Ricetta> GetRicette();
        List<Ricetta> GetRicetteByCollection(int IdCollezione);
        List<Ricetta> GetRicetteByDifficulty(int Difficolta);
        List<Ricetta> GetRicetteByTime(int Tempo);
        Ricetta GetRicetta(int IdRicetta);
        List<Collezione> GetCollezioni();
        Collezione GetCollezione(int IdCollezione);
        List<Collezione> GetDiete();
        List<Utente> GetUtente(int id);
        List<Ricetta> GetMonthRecipes();
        List<Ricetta> GetRicetteSearched(string text);
        List<Collezione> GetCollectionsSearched(int dieta, string text);
        List<CategoriaNutrizionale> GetCategorieNutrizionali(int IdRicetta);
    }
}
