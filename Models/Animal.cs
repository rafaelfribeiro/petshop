using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Petshop.Models
{
    [Table("Animais")]
    class Animal : Model
    {
        public string Nome { get; set; }

        public DateTime DataNasc { get; set; }
		
        public Especie Especie { get; set; }
		
        public Cliente Cliente { get; set; }

    }
}
