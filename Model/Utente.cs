using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.Model
{
    public class Utente
    {
        public int IdUtente { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Pwd { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Esperienza {  get; set; }
        public int Livello { get; set; }
        public string FotoProfilo { get; set; } = string.Empty;
    }
}
