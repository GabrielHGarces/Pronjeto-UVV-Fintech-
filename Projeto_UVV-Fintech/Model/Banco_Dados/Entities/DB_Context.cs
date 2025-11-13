using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_UVV_Fintech.Model.Banco_Dados.Entities
{
    internal class DB_Context : DbContext
    {
        public DbSet<Cliente> ClienteS { get; set; } = null!;
        public DbSet<Conta> ContaS { get; set; } = null!;
        public DbSet<Transacao> TransacoeS { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DBMyAplicacaoFinanceiraEFRelac;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configurar relação 1×N entre Cliente e Conta
            modelBuilder.Entity<Cliente>()
                .HasMany(b => b.Contas)
                .WithOne(c => c.Cliente)
                .HasForeignKey(c => c.ClienteId)
                .IsRequired();

            // Configurar relação 1×N entre Conta e Transações
            modelBuilder.Entity<Conta>()
                .HasMany(b => b.Transacoes)
                .WithOne(c => c.Conta)
                .HasForeignKey(c => c.ContaId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Conta>()
                .Property(p => p.DataCriacao)
                .HasDefaultValueSql("CAST(GETDATE() AS date)");   

            modelBuilder.Entity<Transacao>()
                .Property(p => p.DataHoraTransacao)
                .HasDefaultValueSql("GETDATE()");   // Data/hora atual do SQL Server

            

        }


    }
}
