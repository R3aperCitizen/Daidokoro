using Daidokoro.Model;

namespace Daidokoro.ViewModel
{
    public interface IMainViewModel
    {
        DBService dbService { get; }

        bool InitDBSettings(DBCredentials dbs);

        List<Ricetta> GetRicette();
        List<Ricetta> GetRicette(int IdCollezione);
        List<Collezione> GetCollezioni();
        Collezione GetCollezione(int IdCollezione);
        List<Collezione> GetDiete();
        List<Utente> GetUtente(int id);
        List<Ricetta> GetMonthRecipe();
    }
}
