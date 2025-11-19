using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Projeto_UVV_Fintech.Banco_Dados.Entities;
using Projeto_UVV_Fintech.Repository;
using Projeto_UVV_Fintech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Projeto_UVV_Fintech.Controller
{
    public class ContaController
    {
        private readonly ViewContas _view;

        public ContaController(ViewContas view)
        {
            _view = view;
        }

        //Comentários para evitar erros de compilação pela falta dos métodos em model/Conta.cs
        public bool CriarConta()
        {
            _view.Opacity = 0.5;
            var dialog = new ContaDialog { Owner = _view };
            bool? resultado = dialog.ShowDialog();
            _view.Opacity = 1;

            if (resultado == true)
            {
                int IdCliente = dialog.IdCliente;
                string tipoConta = dialog.tipoConta;

                //if(Cont.CriarConta(IdCliente, tipoConta))
                //{
                //    MessageBox.Show($"Conta criada com sucesso:\nId Cliente: {IdCliente}\nTipo Conta: {tipoConta}");
                //    return true;
                //}
            }
            return false;
        }

        public List<Conta> ListarContas()
        {
 
            List<Conta> resultado = ContaPoupancaRepository.ListarContas();
            List<Conta> resultado2 = ContaCorrenteRepository.ListarContas();
            _view.TabelaContas.ItemsSource = resultado.Concat(resultado2).ToList();
            return resultado;
        }

        public void FiltrarContas(string? IdCliente, int? numerConta, int? numeroAgencia, string? tipoConta, string? nomeTitular, double? saldo, DateTime? dataCriacao, bool? saldoMaior, bool? dataMaior)
        {
            List<Conta> resultado = ContaPoupancaRepository.FiltrarContas(
                int.Parse(IdCliente), numerConta, numeroAgencia, tipoConta,
                nomeTitular, saldo, dataCriacao, saldoMaior, dataMaior);

            _view.TabelaContas.ItemsSource = resultado;
        }

        //public void sacar(Conta conta, double valor)
        //{
        //    if (conta.Sacar(valor))
        //    {
        //        MessageBox.Show("Valor de: R$" + valor + "Sacado com sucesso!");
        //    }
        //}

        //public void depositar(Conta conta, double valor)
        //{
        //    if (conta.Depositar(valor))
        //    {
        //        MessageBox.Show("Valor de: R$" + valor + "Depositado com sucesso!");
        //    }
        //}

        //public void transferir(Conta contaOrigem, Conta contaDestino, double valor)
        //{

        //    if (contaOrigem.Transferir(contaDestino, valor))
        //    {
        //        MessageBox.Show("Valor de: R$" + valor + "Transferido com sucesso para a conta " + contaDestino + "!");
        //    }
        //}

        //public static Conta? ObterContaPorId(int contaId)
        //{
        //    return Conta.ObterContaPorId(contaId);
        //}

        public void AbrirViewClientes(Conta contaSelecionada)
        {
            _view.Hide();
            var window = new ViewClientes(contaSelecionada.Id) { Owner = _view };
            window.ShowDialog();
            _view.Close();
        }

        public void AbrirViewTransacoes(string NumConta)
        {
            _view.Opacity = 0.5;
            var dialog = new ContaTransacaoDialog(int.Parse(NumConta)) { Owner = _view };
            bool? resultado = dialog.ShowDialog();
            _view.Opacity = 1;
        }
    }
}
