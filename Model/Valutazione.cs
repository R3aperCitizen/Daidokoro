namespace Daidokoro.Model
{
    public class Valutazione
    {
        public int IdUtente { get; set; }
        public int IdRicetta { get; set; }
        public int IdCollezione { get; set; }
        public bool Voto { get; set; }
        public DateTime DataValutazione { get; set; }
        public string Commento { get; set; } = string.Empty;
        public string NomeUtente { get; set; } = string.Empty;
        public byte[] FotoUtente { get; set; } = null!;
    }
}
