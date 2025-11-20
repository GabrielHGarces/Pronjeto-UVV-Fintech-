using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;

/*
 * ====================================================================
 * APLICAÇÃO DE BOAS PRÁTICAS: Inversão de Dependência
 * ====================================================================
 *
 * Esta interface funciona como um "contrato" que define as operações
 * que um repositório de cliente deve ser capaz de realizar.
 *
 * O Controller passa a depender deste contrato, e não de uma classe
 * concreta. Isso torna o código mais flexível, pois podemos trocar
 * a implementação do repositório sem quebrar o Controller.
 * Além disso, facilita muito a criação de testes automatizados.
 *
 */
namespace Projeto_UVV_Fintech.Repository.Interfaces
{
    public interface IClienteRepository
    {
        bool CriarCliente(string name, string cEP, string telefone);
        List<Cliente> ListarClientes();
        List<Cliente> FiltrarClientes(int? idCliente, string? telefone, string? cep, string? nomeCliente, int? numeroDeContas, DateTime? dataAdesao, bool? dataMaiorQue);
        Cliente ObterClientePorId(int clienteId);
        List<Cliente> ObterTodosClientes();
    }
}
