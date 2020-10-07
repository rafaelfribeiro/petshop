using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Petshop.Models
{
    class Context : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
		
        public DbSet<Funcionario> Funcionarios { get; set; }
		
        public DbSet<Animal> Animais { get; set; }
		
        public DbSet<Atendimento> Atendimentos { get; set; }
		
        public DbSet<TipoServico> TipoServico { get; set; }
		
        public DbSet<Especie> Especies { get; set; }
		
        public DbSet<Porte> Portes { get; set; }
		
        public DbSet<TipoServico> TiposServico { get; set; }
		
        public DbSet<Servico> Servicos { get; set; }
		
        public DbSet<AtendimentoServicos> AtendimentoServicos { get; set; }
		
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer
                (@"Server=(localdb)\mssqllocaldb;Database=PetshopWPF;Trusted_Connection=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Porte>().HasData(
               new Porte { Id = 1, Nome = "Muito Pequeno", Descricao = "Animais de 0cm a 15cm de comprimento" },
               new Porte { Id = 2, Nome = "Pequeno", Descricao = "Animais de 15,1cm a 25cm de comprimento" },
               new Porte { Id = 3, Nome = "Médio", Descricao = "Animais de 25,5cm a 50cm de comprimento" },
               new Porte { Id = 4, Nome = "Grande", Descricao = "Animais com mais de 50,1cm de comprimento" }
           );

            modelBuilder.Entity<TipoServico>().HasData(
                new TipoServico { Id = 1, Nome = "Estético", Descricao = "Qualquer procedimento que não envolva cirurgia ou medicação" },
                new TipoServico { Id = 2, Nome = "Médico", Descricao = "Qualquer procedimento cirurgico ou medicinal" }
            );

            /*modelBuilder.Entity<Especie>().HasData(
                new Especie { Id = 1, Nome = "Calopsita", Descricao = "Pássaro com topete", PorteId = 1 },
                new Especie { Id = 2, Nome = "Gato", Descricao = "Gato comum", PorteId = 2 },
                new Especie { Id = 3, Nome = "Dachshund", Descricao = "Cachorro salsicha", PorteId = 3 },
                new Especie { Id = 5, Nome = "Labrador Retriever", Descricao = "Cachorro labrador", PorteId = 4 }
            );*/

        }
    }
}
