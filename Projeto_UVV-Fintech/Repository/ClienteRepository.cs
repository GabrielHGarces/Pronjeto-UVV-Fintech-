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
 *    - Esta classe agora implementa a interface `IClienteRepository`.
 *      Isso significa que ela é obrigada a ter todos os métodos que
 *      foram definidos no "contrato", garantindo consistência.
 *
 * 2. MÉTODOS NÃO ESTÁTICOS:
 *    - Os métodos deixaram de ser `static` para que a classe possa ser
 *      instanciada como um objeto. Isso é essencial para que a
 *      "Injeção de Dependência" funcione e para que os testes
 *      sejam facilitados.
 *
 */
namespace Projeto_UVV_Fintech.Repository
{
    public class ClienteRepository : IClienteRepository
    {


        public bool CriarCliente(string name, string cEP, string telefone)
        {

            using var context = new DB_Context();
            Cliente CliNovo = new Cliente();
            CliNovo.Nome = name;
            CliNovo.Telefone = telefone;
            CliNovo.CEP = cEP;


            context.Clientes.Add(CliNovo);
            context.SaveChanges();

            return true;
        }

        public List<Cliente> ListarClientes()
        {
            using var context = new DB_Context();
            return context.Clientes.Include(c => c.Contas).ToList();

        }

        public List<Cliente> FiltrarClientes(int? idCliente, string? telefone, string? cep, string? nomeCliente, int? numeroDeContas, DateTime? dataAdesao, bool? dataMaiorQue)
        {
            List<Cliente> clientes = ListarClientes();
            var filtrado = clientes
                .Where(c =>
                    // Filtra por ID
                    (idCliente == null || c.Id == idCliente)

                    // Filtra por telefone 
                    && (string.IsNullOrWhiteSpace(telefone) ||
                        (!string.IsNullOrWhiteSpace(c.Telefone) && c.Telefone.Contains(telefone)))

                    // Filtra por CEP
                    && (string.IsNullOrWhiteSpace(cep) ||
                        (!string.IsNullOrWhiteSpace(c.CEP) && c.CEP.Contains(cep)))

                    // Filtra por nome
                    && (string.IsNullOrWhiteSpace(nomeCliente) ||
                        (!string.IsNullOrWhiteSpace(c.Nome) && c.Nome.Contains(nomeCliente, StringComparison.OrdinalIgnoreCase)))

                    // Filtra por número de contas
                    && (numeroDeContas == null || c.Contas.Count == numeroDeContas)

                    // Filtra por data de adesão (opcional)
                    && (
                        dataAdesao == null ||  // Se não selecionar data, ignora
                        (
                            dataMaiorQue == true ? c.DataAdesao >= dataAdesao :
                            dataMaiorQue == false ? c.DataAdesao <= dataAdesao :
                            true // Se dataMaiorQue for null, ignora a regra
                        )
                    )
                )
                .ToList();

            return filtrado;
        }


        //Usada em Cliente Controller

        public Cliente ObterClientePorId(int clienteId)
        {
            using var context = new DB_Context();
            return context.Clientes.Find(clienteId);
        }

        public List<Cliente> ObterTodosClientes()
        {
            using var context = new DB_Context();
            return context.Clientes.ToList();
        }
    }

}
