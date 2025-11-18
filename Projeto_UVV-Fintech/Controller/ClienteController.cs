using Projeto_UVV_Fintech.Model;
using Projeto_UVV_Fintech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projeto_UVV_Fintech.Controller
{
    internal class ClienteController
    {
        private readonly ViewContas _view;

        public ClienteController(ViewContas view)
        {
            _view = view;
        }

        //Comentários para evitar erros de compilação pela falta dos métodos em model/Conta.cs
        //public bool CriarCliente(string nome, string telefone, string cep)
        //{
        //    if (Cliente.AdicionarConta(nome, telefone, cep))
        //    {
        //        MessageBox.Show($"Cliente criado com sucesso:\nId Cliente: Nome: {nome}");
        //        return true;
        //    }
        //    return false;
        //}

        //public List<Cliente> ListarClientes()
        //{
        //    List<Cliente> resultado = Cliente.ListarClientes();
        //    _view.TabelaContas.ItemsSource = resultado;
        //    return resultado;
        //}

        //public void FiltrarClientes(int? idCliente, string? telefone, string? cep, string? nomeCliente, int? numeroDeContas, DateTime? dataAdesao, bool? dataMaiorQue)
        //{
        //    List<Cliente> resultado = Cliente.FiltrarContas(
        //        idCliente, telefone, cep, nomeCliente,
        //        numeroDeContas, dataAdesao, dataMaiorQue);

        //    _view.TabelaContas.ItemsSource = resultado;
        //}

        //public static Cliente? ObterContaPorId(int idCliente)
        //{
        //    return Cliente.ObterClientePorId(idCliente);
        //}

    }
}
