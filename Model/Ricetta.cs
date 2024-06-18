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

        public string DifficoltaStella 
        { 
            get 
            {
                string star = string.Empty;
                for (int i = 0; i<Difficolta; i++)
                {
                    star += "⭐";
                }
                return star;
            } 
        }
    }
}
