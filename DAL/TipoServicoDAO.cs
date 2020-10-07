using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Petshop.DAL
{
    class TipoServicoDAO
    {
        private static Context _context = SingletonContext.GetInstance();
        public static TipoServico BuscarPorID(int id) => _context.TiposServico.Find(id);
        public static List<TipoServico> Listar => _context.TiposServico.ToList();
    }
}
