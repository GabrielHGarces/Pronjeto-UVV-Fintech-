using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Projeto_UVV_Fintech.Banco_Dados.Entities;
using Projeto_UVV_Fintech.Repository;
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
                if (tipoConta == "CC")
                {
                    if (ContaCorrenteRepository.CriarConta(idClienteInt))
                    {
                        MessageBox.Show($"Conta criada com sucesso:\nId Cliente: {idClienteInt}\nTipo Conta: {tipoConta}");
                        return true;
                    }
                    return false;
                }
                else if (tipoConta == "CP")
                {
                    if (ContaPoupancaRepository.CriarConta(idClienteInt))
                    {
                        MessageBox.Show($"Conta criada com sucesso:\nId Cliente: {idClienteInt}\nTipo Conta: {tipoConta}");
                        return true;
                    }
                    return false;
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
                List<Conta> Contas = [];
                List<ContaPoupanca> resultadoPoupanca = ContaPoupancaRepository.ListarContas();
                List<ContaCorrente> resultadoCorrente = ContaCorrenteRepository.ListarContas();

                var todasContas = Contas.Concat(resultadoPoupanca).Concat(resultadoCorrente);
                var contasUnicas = todasContas
                    .GroupBy(c => c.Id)
                    .Select(g => g.First())
                    .ToList();
                
                var contasViewModel = contasUnicas.Select(conta => new ContaViewModel
                {
                     ClienteId = conta.Cliente.Id,
                     Agencia = conta.Agencia,
                     NumeroConta = conta.NumeroConta,
                     Tipo = conta.GetType().Name == "ContaCorrente" ? "CC" : "CP",
                     DataCriacao = conta.DataCriacao,
                     Saldo = conta.Saldo,
                     NomeCliente = GetNomeClientePorId(conta.Cliente.Id.ToString())
                }).ToList();

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
                        // Se o ID não for um número válido, limpa a grade e retorna.
                        _view.TabelaContas.ItemsSource = new List<ContaViewModel>();
                        return new List<ContaViewModel>();
                    }
                }

                List<Conta> resultado;
                if (tipoConta == "CP")
                {
                    resultado = ContaPoupancaRepository.FiltrarContas(
                    idClienteInt, numerConta, numeroAgencia, tipoConta,
                    nomeTitular, saldo, dataCriacao, saldoMaior, dataMaior);
                } else if (tipoConta == "CC")
                {
                    resultado = ContaCorrenteRepository.FiltrarContas(
                    idClienteInt, numerConta, numeroAgencia, tipoConta,
                    nomeTitular, saldo, dataCriacao, saldoMaior, dataMaior);
                } else
                {
                    List<Conta> resultadoPoupanca = ContaPoupancaRepository.FiltrarContas(
                    idClienteInt, numerConta, numeroAgencia, tipoConta,
                    nomeTitular, saldo, dataCriacao, saldoMaior, dataMaior);
                    List<Conta> resultadoCorrente = ContaCorrenteRepository.FiltrarContas(
                    idClienteInt, numerConta, numeroAgencia, tipoConta,
                    nomeTitular, saldo, dataCriacao, saldoMaior, dataMaior);
                    resultado = resultadoPoupanca.Concat(resultadoCorrente).ToList();
                }

                var contasViewModel = resultado.Select(conta => new ContaViewModel
                {
                    ClienteId = conta.Cliente.Id,
                    Agencia = conta.Agencia,
                    NumeroConta = conta.NumeroConta,
                    Tipo = conta.GetType().Name == "ContaCorrente" ? "CC" : "CP",
                    DataCriacao = conta.DataCriacao,
                    Saldo = conta.Saldo,
                    NomeCliente = GetNomeClientePorId(conta.Cliente.Id.ToString())
                }).ToList();

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
                Conta? conta = null;
                if (tipoConta == "CC")
                {
                    conta = ContaCorrenteRepository.FiltrarContas(null, numConta, null, null, null, null, null, null, null).FirstOrDefault();
                }
                else if (tipoConta == "CP")
                {
                    conta = ContaPoupancaRepository.FiltrarContas(null, numConta, null, null, null, null, null, null, null).FirstOrDefault();
                }

                if (conta == null)
                {
                    MessageBox.Show("Conta não encontrada.");
                    return;
                }

                bool sucesso = false;
                if (tipoConta == "CC")
                {
                    sucesso = ContaCorrenteRepository.SacarCorrente(conta, valor);
                }
                else if (tipoConta == "CP")
                {
                    sucesso = ContaPoupancaRepository.SacarPoupanca(conta, valor);
                }

                if (sucesso)
                {
                    MessageBox.Show($"Saque de R${valor:F2} realizado com sucesso!");
                }
                else
                {
                    MessageBox.Show("Falha ao realizar o saque. Verifique o saldo e os dados da conta.");
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
                Conta? conta = null;
                if (tipoConta == "CC")
                {
                    conta = ContaCorrenteRepository.FiltrarContas(null, numConta, null, null, null, null, null, null, null).FirstOrDefault();
                }
                else if (tipoConta == "CP")
                {
                    conta = ContaPoupancaRepository.FiltrarContas(null, numConta, null, null, null, null, null, null, null).FirstOrDefault();
                }

                if (conta == null)
                {
                    MessageBox.Show("Conta não encontrada.");
                    return;
                }

                bool sucesso = false;
                if (tipoConta == "CC")
                {
                    sucesso = ContaCorrenteRepository.DepositarCorrente(conta, valor);
                }
                else if (tipoConta == "CP")
                {
                    sucesso = ContaPoupancaRepository.DepositarPoupanca(conta, valor);
                }

                if (sucesso)
                {
                    MessageBox.Show($"Depósito de R${valor:F2} realizado com sucesso!");
                }
                else
                {
                    MessageBox.Show("Falha ao realizar o depósito.");
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
                Conta? contaOrigem = null;
                if (tipoContaOrigem == "CC")
                {
                    contaOrigem = ContaCorrenteRepository.FiltrarContas(null, numContaOrigem, null, null, null, null, null, null, null).FirstOrDefault();
                }
                else if (tipoContaOrigem == "CP")
                {
                    contaOrigem = ContaPoupancaRepository.FiltrarContas(null, numContaOrigem, null, null, null, null, null, null, null).FirstOrDefault();
                }

                if (contaOrigem == null)
                {
                    MessageBox.Show("Conta de origem não encontrada.");
                    return;
                }

                Conta? contaDestino = (Conta?)ContaCorrenteRepository.FiltrarContas(null, numContaDestino, null, null, null, null, null, null, null).FirstOrDefault()
                                     ?? (Conta?)ContaPoupancaRepository.FiltrarContas(null, numContaDestino, null, null, null, null, null, null, null).FirstOrDefault();

                if (contaDestino == null)
                {
                    MessageBox.Show("Conta de destino não encontrada.");
                    return;
                }

                bool sucesso = false;
                if (tipoContaOrigem == "CC")
                {
                    sucesso = ContaCorrenteRepository.TransferirCorrente(contaOrigem, contaDestino, valor);
                }
                else if (tipoContaOrigem == "CP")
                {
                    sucesso = ContaPoupancaRepository.TransferirPoupanca(contaOrigem, contaDestino, valor);
                }

                if (sucesso)
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
                var cliente = ClienteRepository.ObterClientePorId(id);
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
