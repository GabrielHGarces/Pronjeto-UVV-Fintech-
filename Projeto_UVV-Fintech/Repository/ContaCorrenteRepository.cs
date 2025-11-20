using Microsoft.EntityFrameworkCore;
using Projeto_UVV_Fintech.Banco_Dados.Entities;
using Projeto_UVV_Fintech.Repository;
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
            int agencia = 0;
            var clienteAssociado = context.Clientes
                .Include(c => c.Contas)
                .FirstOrDefault(c => c.Id == clienteId);

            if (clienteAssociado == null)
                return false;

            // VERIFICA SE JÁ EXISTE CONTA CORRENTE
            foreach (var conta in clienteAssociado.Contas)
            {
                if (conta != null)
                {
                    agencia = conta.Agencia;
                }
                if (conta is ContaCorrente)
                    return false;
            }

            if (agencia == 0)
            {
                agencia = new Random().Next(10000, 99999);
            }
            Conta novo = new ContaCorrente
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


        public static List<ContaCorrente> ListarContas()
        {
            using var context = new DB_Context();
            return context.Contas.OfType<ContaCorrente>().Include(c => c.Cliente).ToList();
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

        public static bool Sacar(Conta conta, double valor)
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
            else{
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
            IQueryable<Conta> query = context.Contas.OfType<ContaCorrente>().Include(c => c.Cliente);

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
            }
        }
        
        public static void DeletarContaCorrente(int contaId)
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
            }
        }

        public void ExibirDetalhesConta(int contaId)
        {
            using var context = new DB_Context();
            var conta = context.Contas.Find(contaId);
            if (conta != null && conta is ContaCorrente)
            {
            }
            else
            {
            }
        }

        public void BuscarPorClienteId(int clienteId)
        {
            using var context = new DB_Context();
            var contas = context.Contas.Where(c => c.ClienteId == clienteId && c is ContaCorrente).ToList();
            foreach (var conta in contas)
            {
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
            }
        }

        public void BuscarPorSaldoMaiorQue(double saldoMinimo)
        {
            using var context = new DB_Context();
            var contas = context.Contas.Where(c => c is ContaCorrente && c.Saldo > saldoMinimo).ToList();
            foreach (var conta in contas)
            {
            }
        }

        public void BuscarPorSaldoMenorQue(double saldoMaximo)
        {
            using var context = new DB_Context();
            var contas = context.Contas.Where(c => c is ContaCorrente && c.Saldo < saldoMaximo).ToList();
            foreach (var conta in contas)
            {
            }
        }

    }
}
