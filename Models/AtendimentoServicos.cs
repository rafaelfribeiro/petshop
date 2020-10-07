using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Petshop.Models
{
    [Table("AtendimentoServicos")]
    class AtendimentoServicos : Model
    {
        public Servico Servico { get; set; }
		
        public Atendimento Atendimento { get; set; }
    }
}
