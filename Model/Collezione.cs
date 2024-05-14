using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.Model
{
    public class Collezione
    {
        public int IdCollezione { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descrizione { get; set; } = string.Empty;
        public bool Dieta { get; set; }
        public DateTime DataCreazione { get; set; }

        public int IdUtente { get; set; }
        public int IdCategoria { get; set; }
    }
}
