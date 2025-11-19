//using Projeto_UVV_Fintech.Model;
using Projeto_UVV_Fintech.Banco_Dados.Entities;
using Projeto_UVV_Fintech.Repository;
using Projeto_UVV_Fintech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projeto_UVV_Fintech.Controller
{
    public class ClienteController
    {
        private readonly ViewClientes _view;

        public ClienteController(ViewClientes view)
        {
            _view = view;
        }

        //Comentários para evitar erros de compilação pela falta dos métodos em model/Conta.cs
        public bool CriarCliente()
        {
            try
            {
                _view.Opacity = 0.5;
                var dialog = new ClienteDialog(this) { Owner = _view };
                bool? resultado = dialog.ShowDialog();
                _view.Opacity = 1;

                if (resultado == true)
                {
                    ListarClientes();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar o cliente: {ex.Message}");
                return false;
            }
        }

        public bool SalvarCliente(string nome, string telefone, string cep)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("O campo Nome não pode estar vazio.");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(telefone))
            {
                MessageBox.Show("O campo Telefone não pode estar vazio.");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(cep))
            {
                MessageBox.Show("O campo CEP não pode estar vazio.");
                return false;
            }

            try
            {
                if (ClienteRepository.CriarCliente(nome, telefone, cep))
                {
                    MessageBox.Show($"Cliente salvo:\nNome: {nome}\nTelefone: {telefone}\nCEP: {cep}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o cliente: {ex.Message}");
                return false;
            }

            return true;
        }

        public List<Cliente> ListarClientes()
        {
            try
            {
                List<Cliente> resultado = ClienteRepository.ListarClientes();
                _view.TabelaClientes.ItemsSource = resultado;
                return resultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao listar os clientes: {ex.Message}");
                return new List<Cliente>();
            }
        }

        public bool FiltrarClientes(int? idCliente, string? telefone, string? cep, string? nomeCliente, int? numeroDeContas, DateTime? dataAdesao, bool? dataMaiorQue)
        {
            try
            {
                List<Cliente> resultado = ClienteRepository.FiltrarClientes(
                idCliente, telefone, cep, nomeCliente,
                numeroDeContas, dataAdesao, dataMaiorQue);

                _view.TabelaClientes.ItemsSource = resultado;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao filtrar os clientes: {ex.Message}");
                return false;
            }
        }

        public void AbrirViewContas(Cliente clienteSelecionado)
        {
            _view.Hide();
            var window = new ViewContas(clienteSelecionado) { Owner = _view };
            window.ShowDialog();
            _view.Close();
        }

    }
}
