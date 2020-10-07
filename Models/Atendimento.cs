using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Petshop.Models
{
    [Table("Atendimentos")]
    class Atendimento : Model
    {
        public DateTime DataHora { get; set; }

        public Animal Animal { get; set; }

        public Funcionario Funcionario { get; set; }

        public List<AtendimentoServicos> Servicos { get; set; }
		
        public int Duracao { get; set; }
		
        public double Preco { get; set; }

        public Atendimento()
        {
            Servicos = new List<AtendimentoServicos>();
        }

    }
}
