using Projeto_UVV_Fintech.Banco_Dados.Entities;
using Projeto_UVV_Fintech.Repository;
using Projeto_UVV_Fintech.Repository.Interfaces;
using Projeto_UVV_Fintech.ViewModels;
using Projeto_UVV_Fintech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

/*
 * ====================================================================
 * APLICAÇÃO DE BOAS PRÁTICAS NO CONTROLLER
 * ====================================================================
 *
 * 1. INJEÇÃO DE DEPENDÊNCIA (DE FORMA SIMPLES):
 *    - O Controller agora depende do "contrato" `ITransacaoRepository`
 *      e não mais da classe com métodos estáticos.
 *    - Isso torna o controller mais flexível e muito mais fácil
 *      de testar de forma isolada.
 *
 * 2. CÓDIGO SEM REPETIÇÃO ("Don't Repeat Yourself"):
 *    - A lógica para converter uma `Transacao` em um `TransacaoViewModel`
 *      foi centralizada no método `MapToViewModel`, evitando código
 *      duplicado e facilitando a manutenção.
 *
 */
namespace Projeto_UVV_Fintech.Controller
{
    public class TransacaoController
    {
        private readonly ViewTransacoes _view;
        private readonly ITransacaoRepository _transacaoRepository;

        public TransacaoController(ViewTransacoes view)
        {
            _view = view;
            _transacaoRepository = new TransacaoRepository();
        }

        // Método auxiliar para não repetir o código de conversão.
        private TransacaoViewModel MapToViewModel(Transacao transacao)
        {
            return new TransacaoViewModel
            {
                ID = transacao.Id,
                valor = transacao.Valor,
                tipoTransacao = transacao.Tipo.ToString(),
                NumeroContaRemetente = transacao.ContaRemetente?.NumeroConta ?? 0,
                NumeroContaDestinatario = transacao.ContaDestinatario?.NumeroConta ?? 0,
                DataHoraTransacao = transacao.DataHoraTransacao
            };
        }

        //Comentários para evitar erros de compilação pela falta dos métodos em model/Conta.cs
        public bool CriarTransacao(double valor, string tipo, int contaRemetente, int contaDestinatario)
        {
            try
            {
                // A criação de transação continua estática pela razão prática
                // explicada nos comentários do TransacaoRepository.
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

        public List<TransacaoViewModel> ListarTransacoes()
        {
            try
            {
                // A chamada agora é feita através da instância do repositório.
                var transacoes = _transacaoRepository.ListarTransacoes();

                var transacoesViewModel = transacoes.Select(MapToViewModel).ToList();

                _view.TabelaTransacoes.ItemsSource = transacoesViewModel;
                return transacoesViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao listar transações: {ex.Message}");
                return new List<TransacaoViewModel>();
            }
        }





        public List<TransacaoViewModel> FiltrarTransacoes(int? idTransacao, int? contaRemetente, int? contaDestinatario, string? tipo, double? valor, DateTime? dataTransacao, bool? valorMaior, bool? dataMaior)
        {
            try
            {
                List<Transacao> filtrado = _transacaoRepository.FiltrarTransacoes(
                idTransacao, contaRemetente, contaDestinatario,
                tipo, valor, dataTransacao, valorMaior, dataMaior);

                var transacoesViewModel = filtrado.Select(MapToViewModel).ToList();

                _view.TabelaTransacoes.ItemsSource = transacoesViewModel;
                return transacoesViewModel;
            } catch (Exception ex)
            {
                MessageBox.Show($"Erro ao filtrar transacoes: {ex.Message}");
                return new List <TransacaoViewModel>();
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
