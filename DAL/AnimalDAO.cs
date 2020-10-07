using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace Petshop.DAL
{
    class AnimalDAO
    {
        private static Context _context = SingletonContext.GetInstance();

        public static Animal BuscarPorID(int id) => _context.Animais.Find(id);
        public static List<Animal> BuscarPorClienteId(int clienteId)
        {
            List<Animal> lstA = new List<Animal>();

            foreach (Animal a in Listar)
                if (a.Cliente != null && a.Cliente.Id == clienteId)
                    lstA.Add(a);

            return lstA;
        }

        public static List<Animal> Listar => _context.Animais.ToList();

        public static bool Cadastrar(Animal a)
        {           
            _context.Animais.Add(a);
            _context.SaveChanges();
            return true;
        }

        public static bool Remover(Animal a) {
            if (AtendimentoDAO.BuscarPorAnimalId(a.Id).Count > 0)
                return false;

            _context.Animais.Remove(a);
            _context.SaveChanges();
            return true;
        }

        public static void Alterar(Animal a) { _context.Animais.Update(a); _context.SaveChanges(); }

    }
}
