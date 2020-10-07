using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petshop.DAL
{
    class AtendimentoServicosDAO
    {
        private static Context _context = SingletonContext.GetInstance();
        public static List<AtendimentoServicos> Listar => _context.AtendimentoServicos.ToList();
    }
}
