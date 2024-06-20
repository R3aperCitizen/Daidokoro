namespace Daidokoro.Model
{
    public class Valutazione
    {
        public int IdValutazione { get; set; }
        public bool Voto { get; set; }
        public DateTime DataValutazione { get; set; }
        public string Commento { get; set; } = string.Empty;
        public int IdUtente { get; set; }
        public int IdRicetta { get; set; }
        public int IdCollezione { get; set; }
        public string NomeUtente { get; set; } = string.Empty;
        public byte[] FotoUtente { get; set; } = null!;

        public string DataValutazioneString
        {
            get
            {
                return DataValutazione.Day.ToString() + "/" +
                    DataValutazione.Month.ToString() + "/" +
                    DataValutazione.Year.ToString();
            }
        }

        public string VotoToString
        {
            get
            {
                return Voto ? "🍋‍🟩" : "🧅";
            }
        }
    }
}
