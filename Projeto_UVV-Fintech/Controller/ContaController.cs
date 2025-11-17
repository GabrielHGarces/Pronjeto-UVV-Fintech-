using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Projeto_UVV_Fintech.Model;
using Projeto_UVV_Fintech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Projeto_UVV_Fintech.Controller
{
    internal class ContaController
    {
        private readonly ViewContas _view;

        public ContaController(ViewContas view)
        {
            _view = view;
        }

        //Comentários para evitar erros de compilação pela falta dos métodos em model/Conta.cs
        //public Conta CriarConta(int clienteID, string tipoConta)
        //{
        //    if (Conta.AdicionarConta(clienteID, tipoConta))
        //    {
        //        MessageBox.Show($"Conta criada com sucesso:\nId Cliente: {clienteID}\nTipo Conta: {tipoConta}");
        //    }

        //    return novaConta;
        //}

        //public List<Conta> ListarContas()
        //{
        //    List<Conta> resultado = Conta.ListarContas();
        //    _view.TabelaContas.ItemsSource = resultado;
        //    return resultado;
        //}

        //public void FiltrarContas(int? IdCliente, int? numerConta, int? numeroAgencia, string? tipoConta, string? nomeTitular, double? saldo, DateTime? dataCriacao, bool? saldoMaior, bool? dataMaior)
        //{
        //    List<Conta> resultado = Conta.FiltrarContas(
        //        IdCliente, numerConta, numeroAgencia, tipoConta,
        //        nomeTitular, saldo, dataCriacao, saldoMaior, dataMaior);

        //    _view.TabelaContas.ItemsSource = resultado;
        //}

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
    }
}
