namespace Daidokoro.Model
{
    public class Ingrediente
    {
        public int IdIngrediente {  get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descrizione { get; set; } = string.Empty;

        public int IdValoreNutrizionale { get; set; }
        public int IdCategoria { get; set; }
    }
}
