using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Petshop.Models
{
    [Table("Servicos")]
    class Servico : Model
    {
        public string Nome { get; set; }

        public string Descricao { get; set; }
		
        public double Preco { get; set; }

        public int Duracao { get; set; }
		
        public TipoServico TipoServico { get; set; }

    }
}
