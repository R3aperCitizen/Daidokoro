using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
