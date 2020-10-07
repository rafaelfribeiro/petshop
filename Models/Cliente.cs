using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Petshop.Models
{
    [Table("Clientes")]
    class Cliente : Pessoa
    {
        public string Email { get; set; }

        public string Telefone { get; set; }
        public string Endereco { get; set; }

        public List<Animal> Animais { get; set; }

        public Cliente()
        {
            Animais = new List<Animal>();
        }
    }
}
