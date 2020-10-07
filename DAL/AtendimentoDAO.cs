using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Petshop.DAL
{
    class AtendimentoDAO
    {
        private static Context _context = SingletonContext.GetInstance();

        public static Atendimento BuscarPorID(int id) => _context.Atendimentos.Find(id);

        public static List<Atendimento> BuscarPorAnimalId(int animalId)
        {
            List<Atendimento> lstA = new List<Atendimento>();

            foreach (Atendimento a in Listar)
                if (a.Animal.Id == animalId)
                    lstA.Add(a);

            return lstA;
        }
        public static List<Atendimento> BuscarPorAnimalIdEDia(int animalId, DateTime dtAtendimento)
        {
            List<Atendimento> lstA = new List<Atendimento>();

            foreach (Atendimento a in Listar)
                if (a.Animal.Id == animalId && a.DataHora.ToShortDateString().Equals(dtAtendimento.ToShortDateString()))
                    lstA.Add(a);

            return lstA;
        }

        public static List<Atendimento> BuscarPorFuncionarioId(int funcionarioId)
        {
            List<Atendimento> lstA = new List<Atendimento>();

            foreach (Atendimento a in Listar)
                if (a.Funcionario.Id == funcionarioId)
                    lstA.Add(a);

            return lstA;
        }

        public static List<Atendimento> BuscarPorServicoId(int servicoId)
        {
            List<Atendimento> lstA = new List<Atendimento>();

            foreach (Atendimento a in Listar)
                foreach (AtendimentoServicos atSv in a.Servicos)
                    if (atSv.Servico.Id == servicoId && !lstA.Contains(a))
                        lstA.Add(a);

            return lstA;
        }

        public static List<Atendimento> BuscarPorFuncionarioIdEDia(int funcionarioId, DateTime dia)
        {
            List<Atendimento> lstA = new List<Atendimento>();

            foreach (Atendimento a in Listar)
                if (a.Funcionario.Id == funcionarioId && a.DataHora.ToShortDateString().Equals(dia.ToShortDateString()))
                    lstA.Add(a);

            return lstA;
        }

        public static List<Atendimento> Listar => _context.Atendimentos.ToList();

        public static bool Cadastrar(Atendimento a)
        {
            _context.Atendimentos.Add(a);
            _context.SaveChanges();
            return true;
        }

        public static void Remover(Atendimento a) { _context.Atendimentos.Remove(a); _context.SaveChanges(); }

        public static void Alterar(Atendimento a) { _context.Atendimentos.Update(a); _context.SaveChanges(); }
    }
}
