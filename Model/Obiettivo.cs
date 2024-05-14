using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.Model
{
    public class Obiettivo
    {
        public int IdObiettivo {  get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descrizione { get; set; } = string.Empty;
        public int Esperienza { get; set; }
        public int IdCategoria { get; set; }
    }
}
