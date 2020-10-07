using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petshop.DAL
{
    class PorteDAO
    {
        private static Context _context = SingletonContext.GetInstance();
        public static Porte BuscarPorID(int id) => _context.Portes.Find(id);
        public static List<Porte> Listar => _context.Portes.ToList();
    }
}
