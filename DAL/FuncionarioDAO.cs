using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;

namespace Petshop.DAL
{
    class FuncionarioDAO
    {
        private static Context _context = SingletonContext.GetInstance();

        public static Funcionario BuscarPorCPF(string cpf) => _context.Funcionarios.FirstOrDefault(x => x.Cpf.Equals(cpf));

        public static Funcionario BuscarPorID(int id) => _context.Funcionarios.Find(id);

        public static Funcionario BuscarPorMatricula(int matricula) => _context.Funcionarios.FirstOrDefault(x => x.Matricula.Equals(matricula));
        
        public static List<Funcionario> Listar => _context.Funcionarios.ToList();
        
        public static bool Cadastrar(Funcionario f)
        {
            if (BuscarPorCPF(f.Cpf) != null || BuscarPorMatricula(f.Matricula) != null)
                return false;

            _context.Funcionarios.Add(f);
            _context.SaveChanges();
            return true;

        }

        public static bool Remover(Funcionario f)
        {

            if (AtendimentoDAO.BuscarPorFuncionarioId(f.Id).Count > 0)
                return false;

            _context.Funcionarios.Remove(f);
            _context.SaveChanges();
            return true;
        }
        
        public static void Alterar(Funcionario f) { _context.Funcionarios.Update(f); _context.SaveChanges(); }
    }
}
