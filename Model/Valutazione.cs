namespace Daidokoro.Model
{
    public class Valutazione
    {
        public int IdValutazione {  get; set; }
        public short Voto { get; set; }
        public DateTime DataValutazione { get; set; }
        public string Commento { get; set; } = String.Empty;
        public int IdUtente { get; set; }
        public int IdRicetta { get; set; }
        public int IdCollezione { get; set; }   

    }
}
