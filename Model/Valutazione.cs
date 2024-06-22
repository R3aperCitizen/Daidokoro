namespace Daidokoro.Model
{
    public class Valutazione
    {
        public bool Voto { get; set; }
        public int IdUtente { get; set; }
        public int IdRicetta { get; set; }
        public DateTime DataValutazione { get; set; }
        public string Commento { get; set; } = string.Empty;
        public string NomeUtente { get; set; } = string.Empty;
        public byte[] FotoUtente { get; set; } = null!;

        public string DataValutazioneToString
        {
            get
            {
                return DataValutazione.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public string VotoToString
        {
            get { return Voto ? "🍋‍🟩" : "🧅"; }
        }
    }
}
