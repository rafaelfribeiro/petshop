using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petshop.DAL
{
    class EspecieDAO
    {
        private static Context _context = SingletonContext.GetInstance();

        public static Especie BuscarPorID(int id) => _context.Especies.Find(id);

        public static List<Especie> Listar => _context.Especies.ToList();
    }
}
