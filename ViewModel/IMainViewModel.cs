using Daidokoro.Model;

namespace Daidokoro.ViewModel
{
    public interface IMainViewModel
    {
        DBService dbService { get; }

        bool InitDBSettings(string _Server, string _Name, string _User, string _Password);

        List<Ricetta> GetRicette();
    }
}
