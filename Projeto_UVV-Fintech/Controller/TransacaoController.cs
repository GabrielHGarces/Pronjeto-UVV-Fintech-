using Projeto_UVV_Fintech.Views;
using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projeto_UVV_Fintech.Repository;
using System.Windows;
using System.Windows.Navigation;

namespace Projeto_UVV_Fintech.Controller
{
    internal class TransacaoController
    {
        private readonly ViewTransacoes _view;

        public TransacaoController(ViewTransacoes view)
        {
            _view = view;
        }

        //Comentários para evitar erros de compilação pela falta dos métodos em model/Conta.cs
        //public bool CriarTransacao(double valor, string tipo, int contaRemetente, int contaDestinatario)
        //{
        //    if (TransacaoRepository.CriarTransacao(valor, tipo, contaRemetente, contaDestinatario))
        //    {
        //        MessageBox.Show($"Transacao criada com sucesso:\n valor: {valor}\n Tipo: {tipo}\n Remetente: {contaRemetente}\n Destinatario: {contaDestinatario}");
        //        return true;
        //    }
        //    return false;
        //}

        //public List<Transacao> ListarContas()
        //{
        //    List<Transacao> resultado = Transacao.ListarTransacoes();
        //    _view.TabelaTransacoes.ItemsSource = resultado;
        //    return resultado;
        //}

        public void FiltrarTransacoes(int? idTransacao, int? contaRemetente, int? contaDestinatario, string? tipo, double? valor, DateTime? dataTransacao, bool? valorMaior, bool? dataMaior)
        {
            List<Transacao> resultado = TransacaoRepository.FiltrarTransacoes(
                idTransacao, contaRemetente, contaDestinatario,
                tipo, valor, dataTransacao, valorMaior, dataMaior);

            _view.TabelaTransacoes.ItemsSource = resultado;
        }

        //public static Transacao? ObterTransacaoPorId(int transacaoId)
        //{
        //    return Transacao.ObterTransacaoPorId(transacaoId);
        //}
    }
}
