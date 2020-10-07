using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petshop.DAL
{
    class ClienteDAO
    {
        private static Context _context = SingletonContext.GetInstance();
        public static Cliente BuscarPorCPF(string cpf) => _context.Clientes.FirstOrDefault(x => x.Cpf.Equals(cpf));
        public static Cliente BuscarPorID(int id) => _context.Clientes.Find(id);
        public static List<Cliente> Listar => _context.Clientes.ToList();
        public static bool Cadastrar(Cliente c)
        {
            if (BuscarPorCPF(c.Cpf) != null)
                return false;

            _context.Clientes.Add(c);
            _context.SaveChanges();
            return true;

        }
        public static void Remover(Cliente c) { _context.Clientes.Remove(c); _context.SaveChanges(); }
        public static void Alterar(Cliente c) { _context.Clientes.Update(c); _context.SaveChanges(); }
    }
}
