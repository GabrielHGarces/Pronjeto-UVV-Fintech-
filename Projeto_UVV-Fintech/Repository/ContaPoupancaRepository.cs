using Microsoft.EntityFrameworkCore;
using Projeto_UVV_Fintech.Banco_Dados.Entities;
using Projeto_UVV_Fintech.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ====================================================================
 * APLICAÇÃO DE BOAS PRÁTICAS: Implementação do Contrato
 * ====================================================================
 *
 * 1. CUMPRINDO O CONTRATO:
 *    - Esta classe também implementa a interface `IContaRepository`,
 *      assim como a `ContaCorrenteRepository`.
 *
 * 2. CLASSES INTERCAMBIÁVEIS:
 *    - Por implementar o mesmo "contrato", o Controller pode usar
 *      esta classe ou a `ContaCorrenteRepository` da mesma forma,
 *      sem precisar saber dos detalhes de cada uma.
 *
 * 3. NOMES CONSISTENTES E MÉTODOS NÃO ESTÁTICOS:
 *    - Os métodos foram renomeados (ex: `SacarPoupanca` para `Sacar`)
 *      para cumprir o contrato da interface e mantidos como métodos
 *      de instância para permitir o desacoplamento e os testes.
 *
 */
namespace Projeto_UVV_Fintech.Repository
{
    public class ContaPoupancaRepository : IContaRepository
    {
        public bool CriarConta(int clienteId)
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
                
                Cliente = clienteAssociado,
                Agencia = agencia,
                ClienteId = clienteAssociado.Id,

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
            return context.Contas.OfType<ContaPoupanca>().Include(c => c.Cliente).Cast<Conta>().ToList();
        }

        public bool Depositar(int contaId, double valor)
        {
            using var context = new DB_Context();

            var conta = context.Contas
                .OfType<ContaPoupanca>()
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


        public int ObterNumeroContaPorId(int contaId)
        {
            using var context = new DB_Context();

            var conta = context.Contas
                .Where(c => c.Id == contaId)
                .Select(c => c.NumeroConta)
                .FirstOrDefault();

            return conta;
        }

        public bool Sacar(int contaId, double valor)
        {
            using var context = new DB_Context();

            var conta = context.Contas
                .OfType<ContaPoupanca>()
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

        public Conta ObterContaPorNumero(int numeroConta)
        {
            using var context = new DB_Context();

            return context.Contas
                .OfType<ContaPoupanca>()
                .AsNoTracking()
                .FirstOrDefault(c => c.NumeroConta == numeroConta);
        }

        public bool Transferir(int contaOrigemId, int contaDestinoId, double valor)
        {
            using var context = new DB_Context();

            // Buscar as contas normalmente (sem AsNoTracking)
            var contaOrigem = context.Contas
                .OfType<ContaPoupanca>()
                .FirstOrDefault(c => c.Id == contaOrigemId);

            var contaDestino = context.Contas
                .FirstOrDefault(c => c.Id == contaDestinoId); // <-- pode ser corrente ou poupança

            if (contaOrigem == null || contaDestino == null)
                return false;

            if (valor <= 0)
                return false;

            if (contaOrigem.Saldo < valor)
                return false;

            contaOrigem.Saldo -= valor;
            contaDestino.Saldo += valor;


            // Atualizar saldos
            context.SaveChanges();

            // Criar lançamento da transação
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
            IQueryable<Conta> query = context.Contas.OfType<ContaPoupanca>().Include(c => c.Cliente);

            if (idCliente.HasValue)
                query = query.Where(c => c.Cliente.Id == idCliente.Value);

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
