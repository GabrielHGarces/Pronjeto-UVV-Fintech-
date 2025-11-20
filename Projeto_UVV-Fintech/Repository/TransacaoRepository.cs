using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_UVV_Fintech.Repository
{
    internal class TransacaoRepository
    {
        public static bool CriarTransacao(TipoTransacao tipo, double valor, int? contaRemetente, int? contaDestinatario, int contaId)
        {
            using var context = new DB_Context();
            Transacao novo = new Transacao();
            var contaAssociada = context.Contas.Find(contaId);
            novo.Tipo = tipo;
            novo.Valor = valor;
            novo.ContaRemetente = contaRemetente;
            novo.ContaDestinatario = contaDestinatario;
            novo.ContaId = contaId;
            novo.Conta = contaAssociada;
            contaAssociada.Transacoes.Add(novo);

            context.Transacoes.Add(novo);
            context.SaveChanges();

            return true;
        }

        public static List<Transacao> ListarTransacoes()
        {
            using var context = new DB_Context();
            return context.Transacoes.ToList();
            
        }

        public static List<Transacao> FiltrarTransacoes(int? idTransacao, int? contaRemetente, int? contaDestinatario, string? tipo, double? valor, DateTime? dataTransacao, bool? valorMaior, bool? dataMaior)
        {
            var transacoes = ListarTransacoes();

            if (tipo == "Depósito")
            {
                tipo = "Deposito";
            } 
            else if (tipo == "Transferência")
            {
                tipo = "Transferencia";
            }

            var filtrado = transacoes
                .Where(t =>
                    (idTransacao == null || t.Id == idTransacao) &&
                    (contaRemetente == null || t.ContaRemetente == contaRemetente) &&
                    (contaDestinatario == null || t.ContaDestinatario == contaDestinatario) &&
                    (string.IsNullOrWhiteSpace(tipo) || tipo == "Todos" || t.Tipo.ToString().Contains(tipo, StringComparison.OrdinalIgnoreCase)) &&
                    (
                        valor == null ||
                        (
                            valorMaior == true ? t.Valor >= valor :
                            valorMaior == false ? t.Valor <= valor :
                            true
                        )
                    ) &&
                    (
                        dataTransacao == null ||
                        (
                            dataMaior == true ? t.DataHoraTransacao >= dataTransacao :
                            dataMaior == false ? t.DataHoraTransacao <= dataTransacao :
                            true
                        )
                    )
                )
                .ToList();

            return filtrado;
        }



        

    }

}