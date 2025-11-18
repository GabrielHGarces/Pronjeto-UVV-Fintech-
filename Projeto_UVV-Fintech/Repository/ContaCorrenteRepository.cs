using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_UVV_Fintech.Repository
{
    internal class ContaCorrenteRepository 
    {
        public static bool CriarConta(int clienteId)
        {
            using var context = new DB_Context();
            
            Conta novo = new ContaCorrente();
            var clienteAssociado = context.Clientes.Find(clienteId);
            novo.Saldo = 0; // Saldo inicial zero pois está sendo criada zerada
            novo.ClienteId = clienteId;
            novo.Cliente = clienteAssociado;
            clienteAssociado.Contas.Add(novo);

            context.Contas.Add(novo);
            context.SaveChanges();

            return true;
        }

        public static List<Conta> ListarContas()
        {
            using var context = new DB_Context();
            return context.Contas.ToList(); // Ele retorna somente as contas correntes?

            //foreach (var conta in contas)
            //{
            //    MessageBox.Show($"ID: {conta.Id},Tipo de Conta: Corrente, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");

            //}
        }

        public static List<Conta> FiltrarContas(int? IdCliente, int? numerConta, int? numeroAgencia, string? tipoConta, string? nomeTitular, double? saldo, DateTime? dataCriacao, bool? saldoMaior, bool? dataMaior)
        {
            return new List<Conta>(); // adicionar a implementação depois
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
                //MessageBox.Show("Conta Corrente não encontrada.");
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
                //MessageBox.Show("Conta Corrente não encontrada.");
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
                //MessageBox.Show("Conta Corrente não encontrada.");
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
                //MessageBox.Show("Conta Corrente não encontrada.");
            }
        }

        public void ExibirDetalhesConta(int contaId)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaCorrente)
            {
                //MessageBox.Show($"ID: {conta.Id}\nTipo de Conta: Corrente\nSaldo: {conta.Saldo}\nClienteId: {conta.ClienteId}\nData de Criação: {conta.DataCriacao}");
            }
            else
            {
                //MessageBox.Show("Conta Corrente não encontrada.");
            }
        }

        public void BuscarPorClienteId(int clienteId)
        {
            using var context = new DB_Context();
            var contas = context.Contas.Where(c => c.ClienteId == clienteId && c is ContaCorrente).ToList();
            foreach (var conta in contas)
            {
                //MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Corrente, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
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
                //MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Corrente, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
            }
        }

        public void BuscarPorSaldoMaiorQue(double saldoMinimo)
        {
            using var context = new DB_Context();
            var contas = context.Contas.Where(c => c is ContaCorrente && c.Saldo > saldoMinimo).ToList();
            foreach (var conta in contas)
            {
                //MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Corrente, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
            }
        }

        public void BuscarPorSaldoMenorQue(double saldoMaximo)
        {
            using var context = new DB_Context();
            var contas = context.Contas.Where(c => c is ContaCorrente && c.Saldo < saldoMaximo).ToList();
            foreach (var conta in contas)
            {
                //MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Corrente, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
            }
        }

    }
}
