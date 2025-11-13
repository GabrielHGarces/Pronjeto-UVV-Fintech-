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
        // 🔹 Tabelas do banco
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Conta> Contas { get; set; } = null!;
        public DbSet<Transacao> Transacoes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 🔹 Conexão com banco SQL Server LocalDB
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=DBMyAplicacaoFinanceiraEFRelac;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            // 🔹 RELAÇÕES ENTRE ENTIDADES
            

            // Cliente → Conta (1:N)
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Contas)
                .WithOne(c => c.Cliente)
                .HasForeignKey(c => c.ClienteId)
                .IsRequired();

            // Conta → Transação (1:N)
            modelBuilder.Entity<Conta>()
                .HasMany(c => c.Transacoes)
                .WithOne(t => t.Conta)
                .HasForeignKey(t => t.ContaId)
                .IsRequired();

            
            // 🔹 HERANÇA (TPH - Table per Hierarchy)
            

            modelBuilder.Entity<Conta>()
                .HasDiscriminator<string>("TipoConta")
                .HasValue<ContaCorrente>("Corrente")
                .HasValue<ContaPoupanca>("Poupanca");

           
            // 🔹 VALORES PADRÃO (Data e Hora)
            

            // Data de criação da conta (apenas data)
            modelBuilder.Entity<Conta>()
                .Property(p => p.DataCriacao)
                .HasDefaultValueSql("CAST(GETDATE() AS date)");

            // Data e hora da transação (timestamp completo)
            modelBuilder.Entity<Transacao>()
                .Property(p => p.DataHoraTransacao)
                .HasDefaultValueSql("GETDATE()");

            
            // 🔹 TABELAS E NOMES (opcional, mas recomendado)
            
            
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Conta>().ToTable("Contas");
            modelBuilder.Entity<ContaCorrente>().ToTable("Contas");  // mesma tabela (TPH)
            modelBuilder.Entity<ContaPoupanca>().ToTable("Contas");  // mesma tabela (TPH)
            modelBuilder.Entity<Transacao>().ToTable("Transacoes");
        }
    }
}
