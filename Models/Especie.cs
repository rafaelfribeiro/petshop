using System;
using System.Collections.Generic;
using System.Text;

namespace Petshop.Models
{
    class Especie : Model
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }
		
        public Porte Porte { get; set; }
    }
}
