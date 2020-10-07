using System;
using System.Collections.Generic;
using System.Text;

namespace Petshop.Models
{
    class Pessoa : Model
    {
        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public DateTime DataNasc { get; set; }

        public string Cpf { get; set; }

    }
}
