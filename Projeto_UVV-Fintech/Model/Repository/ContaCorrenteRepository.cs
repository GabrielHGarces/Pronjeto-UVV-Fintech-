using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projeto_UVV_Fintech.Model.Repository
{
    internal class ContaCorrenteRepository 
    {
        public void  InserirCorrente(double saldo, int clienteId)
        {
            using var context = new DB_Context();
            
            Conta novo = new ContaCorrente();
            var clienteAssociado = context.Clientes.Find(clienteId);
            novo.Saldo = saldo;
            novo.ClienteId = clienteId;
            novo.Cliente = clienteAssociado;
            clienteAssociado.Contas.Add(novo);

            context.Contas.Add(novo);
            context.SaveChanges();

            



        }

        public void TodasContasCorrentes()
        {
            using var context = new DB_Context();
            var contas = context.Contas.ToList();
            foreach (var conta in contas)
            {
                MessageBox.Show($"ID: {conta.Id},Tipo de Conta: Corrente, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");

            }
        }

        public void AtualizarContaCorrente(int contaId, double novoSaldo)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaCorrente)
            {
                conta.Saldo = novoSaldo;
                context.SaveChanges();
            }
            else
            {
                MessageBox.Show("Conta Corrente não encontrada.");
            }
        }
        
        public void DeletarContaCorrente(int contaId)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaCorrente)
            {
                context.Contas.Remove(conta);
                context.SaveChanges();
            }
            else
            {
                MessageBox.Show("Conta Corrente não encontrada.");
            }
        }

        public double ObterSaldo(int contaId)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaCorrente)
            {
                return conta.Saldo;
            }
            else
            {
                MessageBox.Show("Conta Corrente não encontrada.");
                return 0.0;
            }
        }

        public void AtualizarSaldo(int contaId, double novoSaldo)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaCorrente)
            {
                conta.Saldo = novoSaldo;
                context.SaveChanges();
            }
            else
            {
                MessageBox.Show("Conta Corrente não encontrada.");
            }
        }

        public void ExibirDetalhesConta(int contaId)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaCorrente)
            {
                MessageBox.Show($"ID: {conta.Id}\nTipo de Conta: Corrente\nSaldo: {conta.Saldo}\nClienteId: {conta.ClienteId}\nData de Criação: {conta.DataCriacao}");
            }
            else
            {
                MessageBox.Show("Conta Corrente não encontrada.");
            }
        }

        public void BuscarPorClienteId(int clienteId)
        {
            using var context = new DB_Context();
            var contas = context.Contas.Where(c => c.ClienteId == clienteId && c is ContaCorrente).ToList();
            foreach (var conta in contas)
            {
                MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Corrente, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
            }
        }

        public void BuscarPorClienteId(int clienteId, out List<ContaCorrente> contasCorrente)
        {
            using var context = new DB_Context();
            contasCorrente = context.Contas
                .Where(c => c.ClienteId == clienteId && c is ContaCorrente)
                .Cast<ContaCorrente>()
                .ToList();
        }

        public void BuscarPorTipoConta(int ContaId)
        {
            using var context = new DB_Context();
            var contas = context.Contas.Where(c => c is ContaCorrente).ToList();
            foreach (var conta in contas)
            {
                MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Corrente, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
            }
        }

        public void BuscarPorSaldoMaiorQue(double saldoMinimo)
        {
            using var context = new DB_Context();
            var contas = context.Contas.Where(c => c is ContaCorrente && c.Saldo > saldoMinimo).ToList();
            foreach (var conta in contas)
            {
                MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Corrente, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
            }
        }

        public void BuscarPorSaldoMenorQue(double saldoMaximo)
        {
            using var context = new DB_Context();
            var contas = context.Contas.Where(c => c is ContaCorrente && c.Saldo < saldoMaximo).ToList();
            foreach (var conta in contas)
            {
                MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Corrente, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
            }
        }



    }
}
