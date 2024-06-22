namespace Daidokoro.Model
{
    public class Ricetta
    {
        public int IdRicetta { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descrizione {  get; set; } = string.Empty;
        public string Passaggi { get; set; } = string.Empty;
        public byte[] Foto { get; set; } = null!;
        public int Difficolta { get; set; }
        public int Tempo { get; set; }
        public DateTime DataCreazione { get; set; }
        public int IdUtente { get; set; }
        public long NumeroLike { get; set; }
        public decimal VotiPositivi { get; set; }
        public decimal VotiNegativi { get; set; }
    }
}
