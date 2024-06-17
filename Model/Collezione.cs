namespace Daidokoro.Model
{
    public class Collezione
    {
        public int IdCollezione { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descrizione { get; set; } = string.Empty;
        public bool Dieta { get; set; }
        public DateTime DataCreazione { get; set; }
        public int IdUtente { get; set; }
        public int IdCategoria { get; set; }
        public string NomeCategoria { get; set; } = string.Empty;
        public byte[] FotoRicetta { get; set; } = null!;

        public string DataCreazioneString
        {
            get
            {
                return DataCreazione.Day.ToString() + "/" + 
                    DataCreazione.Month.ToString() + "/" + 
                    DataCreazione.Year.ToString();
            }
        }
    }
}
