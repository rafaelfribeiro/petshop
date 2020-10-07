using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Petshop.Models
{
    [Table("TipoServico")]
    class TipoServico : Model
    {
        public string Nome { get; set; }
		
        public string Descricao { get; set; }
    }
}
