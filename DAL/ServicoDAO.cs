using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petshop.DAL
{
    class ServicoDAO
    {
        private static Context _context = SingletonContext.GetInstance();

        public static Servico BuscarPorID(int id) => _context.Servicos.Find(id);

        public static List<Servico> Listar => _context.Servicos.ToList();

        public static bool Cadastrar(Servico s)
        {           
            _context.Servicos.Add(s);
            _context.SaveChanges();
            return true;

        }

        public static bool Remover(Servico s) {
            if (AtendimentoDAO.BuscarPorServicoId(s.Id).Count > 0)
                return false; 
            _context.Servicos.Remove(s); _context.SaveChanges(); return true; }

        public static void Alterar(Servico s) { _context.Servicos.Update(s); _context.SaveChanges(); }
    }
}
