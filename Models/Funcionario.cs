using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Petshop.Models
{
    [Table("Funcionarios")]
    class Funcionario : Pessoa
    {
        public int Matricula { get; set; }
    }
}
