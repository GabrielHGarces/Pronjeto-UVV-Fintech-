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
    public class TransacaoController
    {
        private readonly ViewTransacoes _view;

        public TransacaoController(ViewTransacoes view)
        {
            _view = view;
        }

        //Comentários para evitar erros de compilação pela falta dos métodos em model/Conta.cs
        public bool CriarTransacao(double valor, string tipo, int contaRemetente, int contaDestinatario)
        {
            try
            {
                //if (TransacaoRepository.CriarTransacao(valor, tipo, contaRemetente, contaDestinatario))
                //{
                //    MessageBox.Show($"Transacao criada com sucesso:\n valor: {valor}\n Tipo: {tipo}\n Remetente: {contaRemetente}\n Destinatario: {contaDestinatario}");
                //    return true;
                //}
                return false;
            } catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar transacao: {ex.Message}");
                return false;
            }
        }

        public List<Transacao> ListarTransacoes()
        {
            try
            {
                List<Transacao> transacoes = TransacaoRepository.ListarTransacoes();
                _view.TabelaTransacoes.ItemsSource = transacoes;
                return transacoes;
            } catch (Exception ex)
            {
                MessageBox.Show($"Erro ao listar transacoes: {ex.Message}");
                return new List<Transacao>();
            }
        }

        public bool FiltrarTransacoes(int? idTransacao, int? contaRemetente, int? contaDestinatario, string? tipo, double? valor, DateTime? dataTransacao, bool? valorMaior, bool? dataMaior)
        {
            try
            {
                List<Transacao> filtrado = TransacaoRepository.FiltrarTransacoes(
                idTransacao, contaRemetente, contaDestinatario,
                tipo, valor, dataTransacao, valorMaior, dataMaior);

                _view.TabelaTransacoes.ItemsSource = filtrado;
                return true;
            } catch (Exception ex)
            {
                MessageBox.Show($"Erro ao filtrar transacoes: {ex.Message}");
                return false;
            }
        }

        public void AbrirViewContas(int numeroConta)
        {
            _view.Hide();
            var window = new ViewContas(numeroConta) { Owner = _view };
            window.ShowDialog();
            _view.Close();
        }
    }
}
