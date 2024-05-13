﻿using System.Text.Json.Nodes;

namespace Daidokoro.Model
{
    public class Ricetta
    {
        public int IdRicetta { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descrizione {  get; set; } = string.Empty;
        public int Difficolta { get; set; }
        public DateTime DataCreazione { get; set; }
        public int IdUtente { get; set; }
    }
}
