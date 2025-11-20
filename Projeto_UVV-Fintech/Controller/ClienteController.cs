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

/*
 * ====================================================================
 * APLICAÇÃO DE BOAS PRÁTICAS NO CONTROLLER
 * ====================================================================
 *
 * 1. INJEÇÃO DE DEPENDÊNCIA (DE FORMA SIMPLES):
 *    - O Controller agora depende do "contrato" `IClienteRepository`,
 *      e não da classe concreta com métodos estáticos.
 *    - A instância é criada no construtor. Isso quebra o acoplamento
 *      rígido que existia.
 *
 * 2. CÓDIGO SEM REPETIÇÃO ("Don't Repeat Yourself"):
 *    - A lógica para converter um `Cliente` em um `ClienteViewModel`
 *      foi colocada em um método separado (`MapToViewModel`).
 *    - Isso evita repetir o mesmo código em `ListarClientes` e
 *      `FiltrarClientes`, simplificando a manutenção.
 *
 */
namespace Projeto_UVV_Fintech.Controller
{
    public class ClienteController
    {
        private readonly ViewClientes _view;
        private readonly IClienteRepository _clienteRepository;

        public ClienteController(ViewClientes view)
        {
            _view = view;
            // A dependência é criada aqui, de forma controlada.
            _clienteRepository = new ClienteRepository();
        }

        // Método auxiliar para não repetir o código de conversão.
        private ClienteViewModel MapToViewModel(Cliente cliente)
        {
            return new ClienteViewModel
            {
                ClientID = cliente.Id,
                ClientName = cliente.Nome,
                Telefone = cliente.Telefone,
                Cep = cliente.CEP,
                DataAdesao = cliente.DataAdesao,
                NumeroContas = cliente.Contas?.Count() ?? 0
            };
        }

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
                // A chamada agora é feita através da interface, e não mais de um método estático.
                if (_clienteRepository.CriarCliente(nome, telefone, cep))
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

        public List<ClienteViewModel> ListarClientes()
        {
            try
            {
                List<Cliente> resultado = _clienteRepository.ListarClientes();

                // Usando o método auxiliar para manter o código limpo.
                var clienteViewModel = resultado.Select(MapToViewModel).ToList();

                _view.TabelaClientes.ItemsSource = clienteViewModel;
                return clienteViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao listar os clientes: {ex.Message}");
                return new List<ClienteViewModel>();
            }
        }




        public List<ClienteViewModel> FiltrarClientes(int? idCliente, string? telefone, string? cep, string? nomeCliente, int? numeroDeContas, DateTime? dataAdesao, bool? dataMaiorQue)
        {
            try
            {
                List<Cliente> resultado = _clienteRepository.FiltrarClientes(
                idCliente, telefone, cep, nomeCliente,
                numeroDeContas, dataAdesao, dataMaiorQue);

                var clienteViewModel = resultado.Select(MapToViewModel).ToList();


                _view.TabelaClientes.ItemsSource = clienteViewModel;
                return clienteViewModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao filtrar os clientes: {ex.Message}");
                return new List<ClienteViewModel>();
            }
        }
        
        public Cliente ObterClientePorId(int id)
        {
            return _clienteRepository.ObterClientePorId(id);
        }

        public void AbrirViewContas(ClienteViewModel clienteSelecionado)
        {
            _view.Hide();
            var window = new ViewContas(clienteSelecionado) { Owner = _view };
            window.ShowDialog();
            _view.Close();
        }

    }
}
