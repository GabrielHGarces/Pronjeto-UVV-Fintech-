using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_UVV_Fintech.Repository
{
    internal class ContaPoupancaRepository
    {
        public bool CriarConta(double saldo, int clienteId)
        {
            using var context = new DB_Context();
            Conta novo = new ContaPoupanca();
            var clienteAssociado = context.Clientes.Find(clienteId);
            novo.Saldo = saldo;
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
            return context.Contas.ToList();
            
        }

        public static List<Conta> FiltrarContas(int? idCliente, int? numeroConta, int? numeroAgencia, string? tipoConta, string? nomeTitular, double? saldo, DateTime? dataCriacao, bool? saldoMaior, bool? dataMaior)
        {
            // Busca todas as contas do BD
            var contas = ListarContas();

            var filtrado = contas
                .Where(c =>
                    // ID do Cliente
                    (idCliente == null || c.ClienteId == idCliente)

                    // Número da conta
                    && (numeroConta == null || c.NumeroConta == numeroConta)

                    // Número da agência
                    && (numeroAgencia == null || c.Agencia == numeroAgencia)

                    && (nomeTitular == null ||
                        (!string.IsNullOrWhiteSpace(c.Cliente.Nome) && c.Cliente.Nome.Contains(nomeTitular, StringComparison.OrdinalIgnoreCase))
                    )


                    // Saldo
                    && (
                        saldo == null ||
                        (
                            saldoMaior == true ? c.Saldo >= saldo :
                            saldoMaior == false ? c.Saldo <= saldo :
                            true
                        )
                    )

                    // Data de criação
                    && (
                        dataCriacao == null ||
                        (
                            dataMaior == true ? c.DataCriacao >= dataCriacao :
                            dataMaior == false ? c.DataCriacao <= dataCriacao :
                            true
                        )
                    )
                )
                .ToList();

            return filtrado;
        }


        public void AtualizarContaPoupanca(int contaId, double novoSaldo)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaPoupanca)
            {
                conta.Saldo = novoSaldo;
                context.SaveChanges();
            }
            else
            {
                //MessageBox.Show("Conta Poupança não encontrada.");
            }
        }

        public void DeletarContaPoupanca(int contaId)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaPoupanca)
            {
                context.Contas.Remove(conta);
                context.SaveChanges();
            }
            else
            {
                //MessageBox.Show("Conta Poupança não encontrada.");
            }
        }

        public ContaPoupanca? ObterContaPorId(int contaId)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaPoupanca poupanca)
            {
                return poupanca;
            }
            return null;
        }

        public List<ContaPoupanca> ObterTodasContasPoupanca()
        {
            using var context = new DB_Context();
            return context.Contas.OfType<ContaPoupanca>().ToList();
        }

        public void BuscarPorId(int contaId)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaPoupanca)
            {
                //MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Poupança, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
            }
            else
            {
                //MessageBox.Show("Conta Poupança não encontrada.");
            }

        }

        public void BuscarPorClienteId(int clienteId)
        {
            using var context = new DB_Context();
            var contas = context.Contas.OfType<ContaPoupanca>().Where(c => c.ClienteId == clienteId).ToList();
            if (contas.Any())
            {
                foreach (var conta in contas)
                {
                    //MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Poupança, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
                }
            }
            else
            {
                //MessageBox.Show("Nenhuma Conta Poupança encontrada para este Cliente.");
            }
        }

        public void BuscarPorTipoConta(int IdConta)
        {
            using var context = new DB_Context();
            var contas = context.Contas.OfType<ContaPoupanca>().ToList();
            foreach (var conta in contas)
            {
                //MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Poupança, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
            }
        }


        public void BuscarPorSaldoMaiorQue(double saldoMinimo)
        {
            using var context = new DB_Context();
            var contas = context.Contas.OfType<ContaPoupanca>().Where(c => c.Saldo > saldoMinimo).ToList();
            if (contas.Any())
            {
                foreach (var conta in contas)
                {
                    //MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Poupança, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
                }
            }
            else
            {
                //MessageBox.Show("Nenhuma Conta Poupança encontrada com saldo maior que o valor especificado.");
            }
        }

        public void BuscarPorSaldoMenorQue(double saldoMaximo)
        {
            using var context = new DB_Context();
            var contas = context.Contas.OfType<ContaPoupanca>().Where(c => c.Saldo < saldoMaximo).ToList();
            if (contas.Any())
            {
                foreach (var conta in contas)
                {
                    //MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Poupança, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
                }
            }
            else
            {
                //MessageBox.Show("Nenhuma Conta Poupança encontrada com saldo menor que o valor especificado.");
            }
        }

        public void BuscarPorNomeCliente(string nomeCliente)
        {
            using var context = new DB_Context();
            var contas = context.Contas.OfType<ContaPoupanca>()
                .Where(c => c.Cliente.Nome.Contains(nomeCliente))
                .ToList();
            if (contas.Any())
            {
                foreach (var conta in contas)
                {
                    //MessageBox.Show($"ID: {conta.Id}, Tipo de Conta: Poupança, Saldo: {conta.Saldo}, ClienteId: {conta.ClienteId}, Data de Criação: {conta.DataCriacao}");
                }
            }
            else
            {
                //MessageBox.Show("Nenhuma Conta Poupança encontrada para o nome de cliente especificado.");
            }
        }
    }
}
