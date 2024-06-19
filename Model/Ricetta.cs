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
        public List<Ingrediente> Ingredienti { get; set; } = new();
        public string Tags {  get; set; } = string.Empty;

        public string DifficoltaStella 
        { 
            get 
            {
                string star = string.Empty;
                for (int i = 0; i<Difficolta; i++)
                {
                    star += "🌶️";
                }
                return star;
            }
        }

        public string IngredientiToString 
        { 
            get
            {
                string _ingredienti = string.Empty;
                for (int i = 0; i<Ingredienti.Count(); i++)
                {
                    _ingredienti += (i+1).ToString() + ". " 
                        + Ingredienti[i].Nome + " - " 
                        + Ingredienti[i].Peso + " gr. \r\n";
                }
                return _ingredienti;
            }
        }
    }
}
