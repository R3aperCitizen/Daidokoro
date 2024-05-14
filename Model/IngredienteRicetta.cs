using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.Model
{
    public class IngredienteRicetta
    {
        public int IdIngrediente { get; set; }
        public int IdRicetta { get; set; }
        public int PesoInGrammi { get; set; }
    }
}
