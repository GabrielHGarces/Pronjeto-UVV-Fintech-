using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Projeto_UVV_Fintech.Banco_Dados.Entities
{
    internal class DB_Context : DbContext
    {
        // 🔹 Tabelas do banco
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Conta> Contas { get; set; } = null!;
        public DbSet<Transacao> Transacoes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // 🔹 Conexão com banco SQL Server LocalDB
            //optionsBuilder.UseSqlServer(
            //    "Server=(localdb)\\mssqllocaldb;Database=DBMyAplicacaoFinanceiraEFRelac;Trusted_Connection=True;");
            var basePath = AppContext.BaseDirectory; // Pasta do EXE
            var dbPath = Path.Combine(basePath, "MeuBanco.db");

            options.UseSqlite($"Data Source={dbPath}");
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
            modelBuilder.Entity<Cliente>()
                .Property(p => p.DataAdesao)
                .HasDefaultValueSql("date('now')");

            // Data de criação da conta (apenas data)
            modelBuilder.Entity<Conta>()
                .Property(p => p.DataCriacao)
                .HasDefaultValueSql("date('now')");

            // Data e hora da transação (timestamp completo)
            modelBuilder.Entity<Transacao>()
                .Property(p => p.DataHoraTransacao)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Transacao>()
                .Property(t => t.Tipo)
                .HasConversion<int>();



            // 🔹 TABELAS E NOMES (opcional, mas recomendado)


            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Conta>().ToTable("Contas");
            modelBuilder.Entity<ContaCorrente>().ToTable("Contas");  // mesma tabela (TPH)
            modelBuilder.Entity<ContaPoupanca>().ToTable("Contas");  // mesma tabela (TPH)
            modelBuilder.Entity<Transacao>().ToTable("Transacoes");
        }
    }
}
