using Microsoft.EntityFrameworkCore;
using Projeto_UVV_Fintech.Banco_Dados.Entities;
using Projeto_UVV_Fintech.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

/*
 * ====================================================================
 * APLICAÇÃO DE BOAS PRÁTICAS: Implementação do Contrato
 * ====================================================================
 *
 * 1. CUMPRINDO O CONTRATO:
 *    - Esta classe agora implementa a interface `IContaRepository`.
 *      Isso a torna uma implementação específica de um "repositório de conta".
 *
 * 2. CLASSES INTERCAMBIÁVEIS:
 *    - Por implementar `IContaRepository`, esta classe se torna "intercambiável"
 *      com `ContaPoupancaRepository`. O Controller pode usar qualquer uma
 *      delas sem saber os detalhes, tratando ambas como um `IContaRepository`.
 *
 * 3. NOMES CONSISTENTES E MÉTODOS NÃO ESTÁTICOS:
 *    - Os métodos foram renomeados (ex: `DepositarCorrente` para `Depositar`)
 *      para ficarem iguais aos definidos na interface.
 *    - Deixaram de ser `static` para permitir a Injeção de Dependência.
 *
 */
namespace Projeto_UVV_Fintech.Repository
{
    public class ContaCorrenteRepository : IContaRepository
    {
        public bool CriarConta(int clienteId)
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
                Cliente = clienteAssociado,
                
               ClienteId = clienteAssociado.Id,
                Agencia = agencia,
                

                NumeroConta = new Random().Next(100000, 999999),
            };

            
            clienteAssociado.Contas.Add(novo);

            context.Contas.Add(novo);
            context.SaveChanges();

            return true;
        }


        public List<Conta> ListarContas()
        {
            using var context = new DB_Context();
            return context.Contas.OfType<ContaCorrente>().Include(c => c.Cliente).Cast<Conta>().ToList();
        }

        public bool Depositar(int contaId, double valor)
        {
            using var context = new DB_Context();

            var conta = context.Contas
                .OfType<ContaCorrente>()
                .FirstOrDefault(c => c.Id == contaId);

            if (conta == null)
                return false;

            conta.Saldo += valor;

            context.SaveChanges();

            TransacaoRepository.CriarTransacao(
                TipoTransacao.Deposito, valor, conta.Id, conta.Id
            );

            return true;
        }

        public Conta ObterContaPorNumero(int numeroConta)
        {
            using var context = new DB_Context();

            return context.Contas
                .OfType<ContaCorrente>()
                .AsNoTracking()  // 👈 ESSENCIAL
                .FirstOrDefault(c => c.NumeroConta == numeroConta);
        }

        public bool Sacar(int contaId, double valor)
        {
            using var context = new DB_Context();

            var conta = context.Contas
                .OfType<ContaCorrente>()
                .FirstOrDefault(c => c.Id == contaId);

            if (conta == null || conta.Saldo < valor)
                return false;

            conta.Saldo -= valor;

            context.SaveChanges();

            TransacaoRepository.CriarTransacao(
                TipoTransacao.Saque, valor, conta.Id, conta.Id
            );

            return true;
        }


        public int ObterNumeroContaPorId(int contaId)
        {
            using var context = new DB_Context();

            var conta = context.Contas
                .Where(c => c.Id == contaId)
                .Select(c => c.NumeroConta)
                .FirstOrDefault();

            return conta;
        }

        public bool Transferir(int contaOrigemId, int contaDestinoId, double valor)
        {
            using var context = new DB_Context();

            // Origem sempre é CC
            var contaOrigem = context.Contas
                .OfType<ContaCorrente>()
                .FirstOrDefault(c => c.Id == contaOrigemId);

            // Destino pode ser CC ou CP
            var contaDestino = context.Contas
                .FirstOrDefault(c => c.Id == contaDestinoId);

            if (contaOrigem == null || contaDestino == null)
                return false;

            if (valor <= 0)
                return false;

            if (contaOrigem.Saldo < valor)
                return false;

            contaOrigem.Saldo -= valor;
            contaDestino.Saldo += valor;

            // Gravar alterações de saldo
            context.SaveChanges();

            // Criar registro de transação
            TransacaoRepository.CriarTransacao(
                TipoTransacao.Transferencia,
                valor,
                contaOrigem.Id,
                contaDestino.Id
            );

            return true;
        }



        public List<Conta> FiltrarContas(int? idCliente, int? numeroConta, int? numeroAgencia, string? tipoConta, string? nomeTitular, double? saldo, DateTime? dataCriacao, bool? saldoMaior, bool? dataMaior)
        {
            using var context = new DB_Context();
            IQueryable<Conta> query = context.Contas.OfType<ContaCorrente>().Include(c => c.Cliente);

            if (idCliente.HasValue)
                query = query.Where(c => c.Cliente.Id== idCliente.Value);

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


        

    }
}
