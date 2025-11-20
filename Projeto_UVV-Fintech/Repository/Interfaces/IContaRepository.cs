using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;

/*
 * ====================================================================
 * APLICAÇÃO DE BOAS PRÁTICAS: Contratos e Classes Intercambiáveis
 * ====================================================================
 *
 * Esta interface, além de definir um "contrato" (Princípio da Inversão
 * de Dependência), também permite que diferentes tipos de repositórios
 * de conta (Corrente, Poupança) sejam tratados da mesma forma.
 *
 * Qualquer classe que assine este contrato pode ser substituída por
 * outra que também assine, sem quebrar o sistema. Este é o
 * "Princípio da Substituição de Liskov".
 *
 * É isso que nos permite tratar uma Conta Corrente e uma Poupança
 * de forma genérica no Controller, tornando o código muito mais
 * limpo e flexível.
 *
 */
namespace Projeto_UVV_Fintech.Repository.Interfaces
{
    public interface IContaRepository
    {
        bool CriarConta(int clienteId);
        List<Conta> ListarContas();
        bool Depositar(int contaId, double valor);
        Conta ObterContaPorNumero(int numeroConta);
        bool Sacar(int contaId, double valor);
        bool Transferir(int contaOrigemId, int contaDestinoId, double valor);
        List<Conta> FiltrarContas(int? idCliente, int? numeroConta, int? numeroAgencia, string? tipoConta, string? nomeTitular, double? saldo, DateTime? dataCriacao, bool? saldoMaior, bool? dataMaior);
    }
}
