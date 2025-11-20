using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;

/*
 * ====================================================================
 * APLICAÇÃO DE BOAS PRÁTICAS: Inversão de Dependência
 * ====================================================================
 *
 * Este é o "contrato" para o repositório de transações.
 *
 * Assim como as outras interfaces, seu objetivo é desacoplar o Controller
 * da implementação concreta do repositório, tornando o sistema
 * mais flexível e fácil de testar.
 *
 */
namespace Projeto_UVV_Fintech.Repository.Interfaces
{
    public interface ITransacaoRepository
    {
        List<Transacao> ListarTransacoes();
        List<Transacao> FiltrarTransacoes(int? idTransacao, int? contaRemetente, int? contaDestinatario, string? tipo, double? valor, DateTime? dataTransacao, bool? valorMaior, bool? dataMaior);
    }
}
