﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daidokoro.Model
{
    public class Valore_nutrizionale
    {
        public int IdValoreNutrizionale { get; set; }
        public float Calorie { get; set; }
        public float Grassi { get; set; }
        public float Grassi_Saturi {  get; set; }
        public float Carboidrati { get; set; }
        public float Zucchero {  get; set; }
        public float Fibre {  get; set; }
        public float Proteine { get; set; }
        public float Sale {  get; set; }
        public int IdIngrediente {  get; set; }
    }
}
