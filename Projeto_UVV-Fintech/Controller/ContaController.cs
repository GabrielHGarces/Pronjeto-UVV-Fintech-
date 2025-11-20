using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
using System.Windows.Controls;
using System.Windows.Documents;
using static Projeto_UVV_Fintech.Views.ContaTransacaoDialog;

/*
 * ====================================================================
 * APLICAÇÃO DE BOAS PRÁTICAS NO CONTROLLER
 * ====================================================================
 *
 * Este controller foi o principal alvo da refatoração para deixar o
 * código mais flexível e fácil de manter.
 *
 * PONTO PRINCIPAL: O "PRINCÍPIO ABERTO/FECHADO"
 * --------------------------------------------------------------------
 * Antes, o código usava vários `if/else` para tratar contas do tipo
 * "CC" e "CP". Se um novo tipo de conta fosse adicionado, teríamos
 * que alterar o código em vários lugares (o que é arriscado).
 *
 * A solução foi usar um Dicionário que "mapeia" o tipo da conta para
 * o repositório que sabe cuidar dela. Agora, para adicionar um novo
 * tipo de conta, basta criar o novo repositório e registrá-lo no
 * dicionário. Nenhuma outra parte do controller precisa ser modificada.
 *
 * Isso significa que a classe está "aberta para extensão" (aceita
 * novos tipos de conta) mas "fechada para modificação" (o código
 * principal não é alterado).
 *
 * PONTO SECUNDÁRIO: CÓDIGO SEM REPETIÇÃO ("Don't Repeat Yourself")
 * --------------------------------------------------------------------
 * A lógica para converter (mapear) uma `Conta` para um `ContaViewModel`
 * estava duplicada. Ela foi movida para um método auxiliar `MapToViewModel`
 * para ser reutilizada, simplificando a manutenção.
 *
 */
namespace Projeto_UVV_Fintech.Controller
{
    public class ContaController
    {
        private readonly ViewContas _view;
        private readonly IClienteRepository _clienteRepository;

        // Este dicionário é a solução para evitar os `if/else`.
        // Ele funciona como um "catálogo" de repositórios.
        private readonly Dictionary<string, IContaRepository> _contaRepositories;

        // BOA PRÁTICA: Usar constantes evita erros de digitação com "CC" e "CP".
        private const string ContaCorrente = "CC";
        private const string ContaPoupanca = "CP";

        public ContaController(ViewContas view)
        {
            _view = view;
            _clienteRepository = new ClienteRepository();

            // Aqui, "preenchemos o catálogo" com os repositórios que o sistema conhece.
            // Para adicionar um novo tipo de conta, a mudança seria apenas aqui.
            _contaRepositories = new Dictionary<string, IContaRepository>
            {
                { ContaCorrente, new ContaCorrenteRepository() },
                { ContaPoupanca, new ContaPoupancaRepository() }
            };
        }

        // Método auxiliar para não repetir o código de conversão.
        private ContaViewModel MapToViewModel(Conta conta)
        {
            return new ContaViewModel
            {
                ClienteId = conta.Cliente.Id,
                Agencia = conta.Agencia,
                NumeroConta = conta.NumeroConta,
                Tipo = conta.GetType().Name == "ContaCorrente" ? ContaCorrente : ContaPoupanca,
                DataCriacao = conta.DataCriacao,
                Saldo = conta.Saldo,
                NomeCliente = GetNomeClientePorId(conta.Cliente.Id.ToString())
            };
        }

        public bool CriarConta()
        {
            try
            {
                _view.Opacity = 0.5;
                var dialog = new ContaDialog(this) { Owner = _view };
                bool? resultado = dialog.ShowDialog();
                _view.Opacity = 1;

                if (resultado == true)
                {
                    ListarContas();
                    return true;
                }
                return false;
            } catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar conta: " + ex.Message);
                return false;
            }
        }

        public bool SalvarConta(string idCliente, string tipoConta, string nomeCliente)
        {
            if (string.IsNullOrWhiteSpace(idCliente))
            {
                MessageBox.Show("O campo ID não pode estar vazio.");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(tipoConta))
            {
                MessageBox.Show("O campo Tipo de Conta não pode estar vazio.");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(nomeCliente))
            {
                MessageBox.Show("Adicione o id correto do Cliente.");
                return false;
            }

            int idClienteInt = int.Parse(idCliente);

            try
            {
                // Em vez de um `if`, buscamos no "catálogo" o repositório certo.
                if (_contaRepositories.TryGetValue(tipoConta, out var repository))
                {
                    if (repository.CriarConta(idClienteInt))
                    {
                        MessageBox.Show($"Conta criada com sucesso:\nId Cliente: {idClienteInt}\nTipo Conta: {tipoConta}");
                        return true;
                    }
                }
                return false;
            } catch (Exception ex)
            {
                MessageBox.Show("ID do Cliente inválido. Por favor, insira um número válido." + ex.Message);
                return false;
            }
        }

        public void ListarContas()
        {
            try
            {
                // Para listar tudo, simplesmente pegamos todos os repositórios do catálogo.
                var todasContas = _contaRepositories.Values.SelectMany(repo => repo.ListarContas());

                var contasUnicas = todasContas
                    .GroupBy(c => c.Id)
                    .Select(g => g.First())
                    .ToList();
                
                var contasViewModel = contasUnicas.Select(MapToViewModel).ToList();

                _view.TabelaContas.ItemsSource = contasViewModel;
            } catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar contas: " + ex.Message);
            }
        }

        public List<ContaViewModel> FiltrarContas(string? IdCliente, int? numerConta, int? numeroAgencia, string? tipoConta, string? nomeTitular, double? saldo, DateTime? dataCriacao, bool? saldoMaior, bool? dataMaior)
        {
            try
            {
                int? idClienteInt = null;
                if (!string.IsNullOrEmpty(IdCliente))
                {
                    if (int.TryParse(IdCliente, out int parsedId))
                    {
                        idClienteInt = parsedId;
                    }
                    else
                    {
                        _view.TabelaContas.ItemsSource = new List<ContaViewModel>();
                        return new List<ContaViewModel>();
                    }
                }

                List<Conta> resultado;
                // Se o filtro especificar um tipo, pegamos o repositório certo do catálogo.
                if (!string.IsNullOrEmpty(tipoConta) && _contaRepositories.TryGetValue(tipoConta, out var repository))
                {
                    resultado = repository.FiltrarContas(
                    idClienteInt, numerConta, numeroAgencia, tipoConta,
                    nomeTitular, saldo, dataCriacao, saldoMaior, dataMaior);
                }
                else // Senão, pedimos para todos os repositórios do catálogo filtrarem.
                {
                    resultado = _contaRepositories.Values.SelectMany(repo => repo.FiltrarContas(
                        idClienteInt, numerConta, numeroAgencia, null,
                        nomeTitular, saldo, dataCriacao, saldoMaior, dataMaior)).ToList();
                }

                var contasViewModel = resultado.Select(MapToViewModel).ToList();

                _view.TabelaContas.ItemsSource = contasViewModel;
                return contasViewModel;
            } catch (Exception ex)
            {
                MessageBox.Show("Erro ao filtrar contas: " + ex.Message);
                return new List<ContaViewModel>();
            }
            
        }

        public void Sacar(int numConta, string tipoConta, double valor)
        {
            try
            {
                if (_contaRepositories.TryGetValue(tipoConta, out var repository))
                {
                    var conta = repository.ObterContaPorNumero(numConta);
                    if (conta == null)
                    {
                        MessageBox.Show("Conta não encontrada.");
                        return;
                    }

                    if (repository.Sacar(conta.Id, valor))
                    {
                        MessageBox.Show($"Saque de R${valor:F2} realizado com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Falha ao realizar o saque. Verifique o saldo e os dados da conta.");
                    }
                }
                else
                {
                    MessageBox.Show("Tipo de conta inválido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao realizar saque: " + ex.Message);
            }
        }

        public void Depositar(int numConta, string tipoConta, double valor)
        {
            try
            {
                if (_contaRepositories.TryGetValue(tipoConta, out var repository))
                {
                    var conta = repository.ObterContaPorNumero(numConta);
                    if (conta == null)
                    {
                        MessageBox.Show("Conta não encontrada.");
                        return;
                    }
                    
                    if (repository.Depositar(conta.Id, valor))
                    {
                        MessageBox.Show($"Depósito de R${valor:F2} realizado com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Falha ao realizar o depósito.");
                    }
                }
                else
                {
                    MessageBox.Show("Tipo de conta inválido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao realizar depósito: " + ex.Message);
            }
        }

        public void Transferir(int numContaOrigem, string tipoContaOrigem, int numContaDestino, double valor)
        {
            try
            {
                if (!_contaRepositories.TryGetValue(tipoContaOrigem, out var origemRepository))
                {
                    MessageBox.Show("Tipo de conta de origem inválido.");
                    return;
                }

                var contaOrigem = origemRepository.ObterContaPorNumero(numContaOrigem);
                if (contaOrigem == null)
                {
                    MessageBox.Show("Conta de origem não encontrada.");
                    return;
                }

                // Busca a conta de destino em todos os repositórios conhecidos.
                Conta contaDestino = null;
                foreach (var repo in _contaRepositories.Values)
                {
                    contaDestino = repo.ObterContaPorNumero(numContaDestino);
                    if (contaDestino != null) break;
                }

                if (contaDestino == null)
                {
                    MessageBox.Show("Conta de destino não encontrada.");
                    return;
                }

                if (origemRepository.Transferir(contaOrigem.Id, contaDestino.Id, valor))
                {
                    MessageBox.Show($"Transferência de R${valor:F2} para a conta {numContaDestino} realizada com sucesso!");
                }
                else
                {
                    MessageBox.Show("Falha ao realizar a transferência. Verifique os dados e o saldo.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao realizar transferência: " + ex.Message);
            }
        }

        public string GetNomeClientePorId(string idCliente)
        {
            if (int.TryParse(idCliente, out int id))
            {
                var cliente = _clienteRepository.ObterClientePorId(id);
                return cliente?.Nome ?? string.Empty;
            }
            return string.Empty;
        }

        public void AbrirViewClientes(ContaViewModel contaSelecionada)
        {
            _view.Hide();
            var window = new ViewClientes(contaSelecionada.ClienteId) { Owner = _view };
            window.ShowDialog();
            _view.Close();
        }

        public void AbrirViewTransacoes(string NumConta)
        {
            _view.Opacity = 0.5;
            var dialog = new ContaTransacaoDialog(int.Parse(NumConta), this) { Owner = _view };
            bool? resultado = dialog.ShowDialog();
            _view.Opacity = 1;
        }

        public void AbrirTransacoes(string NumConta)
        {
            _view.Hide();
            var window = new ViewTransacoes(int.Parse(NumConta)) { Owner = _view };
            window.ShowDialog();
            _view.Close();
        }

        public bool AbrirSaqueDialog(Window owner, int numConta, string tipoConta)
        {
            owner.Opacity = 0.5;
            var dialog = new SaqueDialog(this, numConta, tipoConta) { Owner = owner };
            bool? result = dialog.ShowDialog();
            owner.Opacity = 1;
            if (result == true)
            {
                ListarContas();
                return true;
            }
            return false;
        }

        public bool AbrirDepositoDialog(Window owner, int numConta, string tipoConta)
        {
            owner.Opacity = 0.5;
            var dialog = new DepositoDialog(this, numConta, tipoConta) { Owner = owner };
            bool? result = dialog.ShowDialog();
            owner.Opacity = 1;
            if (result == true)
            {
                ListarContas();
                return true;
            }
            return false;
        }

        public bool AbrirTransferenciaDialog(Window owner, int numConta, string tipoConta)
        {
            owner.Opacity = 0.5;
            var dialog = new TransferenciaDialog(this, numConta, tipoConta) { Owner = owner };
            bool? result = dialog.ShowDialog();
            owner.Opacity = 1;
            if (result == true)
            {
                ListarContas();
                return true;
            }
            return false;
        }
    }
}
