using Microsoft.EntityFrameworkCore;
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
        public static bool CriarConta(int clienteId)
        {
            using var context = new DB_Context();
            int agencia =0;
            var clienteAssociado = context.Clientes
                .Include(c => c.Contas)
                .FirstOrDefault(c => c.Id == clienteId);

            if (clienteAssociado == null)
                return false;

            // VERIFICA SE JÁ EXISTE Uma Conta Poupança
            foreach (var conta in clienteAssociado.Contas)
            {
                if (conta != null)
                {
                    agencia = conta.Agencia;
                }

                if (conta is ContaPoupanca)
                    return false;
            }
            if (agencia == 0)
            {
                agencia = new Random().Next(10000, 99999);
            }

            Conta novo = new ContaPoupanca
            {
                Saldo = 0,
                ClienteId = clienteId,
                Cliente = clienteAssociado,
                Agencia = agencia,
                NumeroConta = new Random().Next(100000, 999999),
            };

            clienteAssociado.NumeroContasCliente++;
            clienteAssociado.Contas.Add(novo);

            context.Contas.Add(novo);
            context.SaveChanges();

            return true;
        }

        public static List<ContaPoupanca> ListarContas()
        {
            using var context = new DB_Context();
            return context.Contas.OfType<ContaPoupanca>().Include(c => c.Cliente).ToList();
        }

        public static bool Depositar(Conta conta, double valor)
        {
            if (conta == null)
            {
                return false;
            }



            conta.Saldo += valor;
            using var context = new DB_Context();
            context.Contas.Update(conta);

            context.SaveChanges();
            TransacaoRepository.CriarTransacao(TipoTransacao.Deposito, valor, conta.Id, conta.Id, conta.Id);
            return true;




        }

        public static bool  Sacar(Conta conta, double valor)
        {
            if (conta == null || conta.Saldo < valor)
            {
                return false;
            }



            conta.Saldo -= valor;
            using var context = new DB_Context();
            context.Contas.Update(conta);
            context.SaveChanges();
            TransacaoRepository.CriarTransacao(TipoTransacao.Deposito, valor, conta.Id, conta.Id, conta.Id);
            return true;

        }

        public static bool Transferir(Conta contaOrigem, Conta contaDestino, double valor)
        {
            if (contaOrigem == null || contaDestino == null)
                return false;




            if (contaOrigem.Saldo < valor)
            {

                return false;
            }
            else
            {
                using var context = new DB_Context();

                contaOrigem.Saldo -= valor;
                contaDestino.Saldo += valor;

                context.Contas.Update(contaOrigem);
                context.Contas.Update(contaDestino);


                context.SaveChanges();
                TransacaoRepository.CriarTransacao(TipoTransacao.Deposito, valor, contaOrigem.Id, contaDestino.Id, contaOrigem.Id);
                return true;
            }


        }


        public static List<Conta> FiltrarContas(int? idCliente, int? numeroConta, int? numeroAgencia, string? tipoConta, string? nomeTitular, double? saldo, DateTime? dataCriacao, bool? saldoMaior, bool? dataMaior)
        {
            using var context = new DB_Context();
            IQueryable<Conta> query = context.Contas.OfType<ContaPoupanca>().Include(c => c.Cliente);

            if (idCliente.HasValue)
                query = query.Where(c => c.ClienteId == idCliente.Value);

            if (numeroConta.HasValue)
                query = query.Where(c => c.NumeroConta == numeroConta.Value);

            if (numeroAgencia.HasValue)
                query = query.Where(c => c.Agencia == numeroAgencia.Value);

            if (!string.IsNullOrWhiteSpace(nomeTitular))
                query = query.Where(c => c.Cliente.Nome.ToUpper().Contains(nomeTitular.ToUpper()));

            if (saldo.HasValue)
            {
                if (saldoMaior == true)
                    query = query.Where(c => c.Saldo >= saldo.Value);
                else
                    query = query.Where(c => c.Saldo <= saldo.Value);
            }

            if (dataCriacao.HasValue)
            {
                if (dataMaior == true)
                    query = query.Where(c => c.DataCriacao >= dataCriacao.Value);
                else
                    query = query.Where(c => c.DataCriacao <= dataCriacao.Value);
            }

            return query.ToList();
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
            }
        }

        public static void DeletarContaPoupanca(int contaId)
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
            }
            else
            {
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
                }
            }
            else
            {
            }
        }

        public void BuscarPorTipoConta(int IdConta)
        {
            using var context = new DB_Context();
            var contas = context.Contas.OfType<ContaPoupanca>().ToList();
            foreach (var conta in contas)
            {
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
                }
            }
            else
            {
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
                }
            }
            else
            {
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
                }
            }
            else
            {
            }
        }
    }
}
