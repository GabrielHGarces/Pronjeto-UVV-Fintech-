using Projeto_UVV_Fintech.Banco_Dados.Entities;
using Projeto_UVV_Fintech.Controller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Projeto_UVV_Fintech.Views.ViewClientes;

namespace Projeto_UVV_Fintech.Views
{
    public partial class ViewContas : Window
    {
        //Filters
        public int? numeroConta = null;
        public int? numeroAgencia = null;
        public string? tipoConta = "Todos";
        public string? nomeTitular = "";
        public double saldo = 0;
        public DateTime? dataSelecionada = null;
        public bool saldoMaiorQue = true;
        public bool dataMaiorQue = true;

        public ContaController contaController;

        public ViewContas()
        {
            InitializeComponent();
            contaController = new ContaController(this);

            TabelaContas.ItemsSource = contaController.ListarContas();
        }

        public ViewContas(int nConta)
        {
            InitializeComponent();
            contaController = new ContaController(this);

            NConta.Text = nConta.ToString();
            numeroConta = nConta;


            TabelaContas.ItemsSource = contaController.ListarContas();
            PerformSearch();
        }
        public ViewContas(Cliente clienteSelecionado)
        {
            InitializeComponent();
            contaController = new ContaController(this);

            ClienteID.Text = clienteSelecionado.Id.ToString();

            TabelaContas.ItemsSource = contaController.ListarContas();
            PerformSearch();
        }


        private void DataInput_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dataSelecionada = DataInput.SelectedDate;
        }

        private void NumericTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SaldoD.Text = "";
        }

        private void NumericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(SaldoD.Text, out saldo))
            {
                saldo = double.Parse(SaldoD.Text);
            }
            else
            {
                Console.WriteLine("Erro ao converter para decimal!!!");
            }

            // Formata o valor como moeda brasileira (R$)
            string valorFormatado = String.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", saldo);

            // Exibe o resultado: R$ 123.456,78
            Console.WriteLine(valorFormatado);

            SaldoD.Text = valorFormatado;
            if (saldo == 0)
            {
                SaldoD.Text = "";
            }
        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            bool isDigit = Char.IsDigit(e.Text, 0);

            string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;

            bool isSeparator = e.Text == ",";

            if (isDigit)
            {
                e.Handled = false;
            }
            else if (isSeparator)
            {
                if (textBox.Text.Contains(","))
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void NumericTextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!System.Text.RegularExpressions.Regex.IsMatch(text, "[0-9]+"))
                {
                    e.CancelCommand(); // Cancela o comando de colar
                }
            }
            else
            {
                e.CancelCommand(); // Cancela se não for texto
            }
        }

        private void AdicionarConta_Click(object sender, RoutedEventArgs e)
        {
            contaController.CriarConta();
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NAgencia_TextChanged(object sender, TextChangedEventArgs e)
        {
            numeroAgencia = int.TryParse(NAgencia.Text, out int NumAgencia) ? NumAgencia : null;
        }

        private void NConta_TextChanged(object sender, TextChangedEventArgs e)
        {
            numeroConta = int.TryParse(NConta.Text, out int nContas) ? nContas : null;
        }

        private void NomeCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            nomeTitular = NomeCliente.Text;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            //if (contaController == null)
            //{
            //    TabelaContas.ItemsSource = null;
            //    return;
            //}

            //var query = contas.AsEnumerable();

            //// Filtrar por ID do cliente (se informado)
            //if (!string.IsNullOrWhiteSpace(ClienteID.Text))
            //{
            //    string buscaID = ClienteID.Text;
            //    query = query.Where(p => p.IdCliente.ToString().Contains(buscaID));
            //}

            //// Filtrar por número da conta (se informado)
            //if (numeroConta.HasValue)
            //{
            //    string buscaConta = numeroConta.Value.ToString();
            //    query = query.Where(p => p.NumConta.ToString().Contains(buscaConta));
            //}

            //// Filtrar por número da agência (se informado)
            //if (numeroAgencia.HasValue)
            //{
            //    string buscaAgencia = numeroAgencia.Value.ToString();
            //    query = query.Where(p => p.Agencia.ToString().Contains(buscaAgencia));
            //}

            //// Filtrar por tipo de conta (se não for "Todos" e não for nulo/vazio)
            //if (!string.IsNullOrWhiteSpace(tipoConta) && !string.Equals(tipoConta, "Todos", StringComparison.OrdinalIgnoreCase))
            //{
            //    query = query.Where(p => !string.IsNullOrWhiteSpace(p.Tipo) &&
            //                         p.Tipo.IndexOf(tipoConta, StringComparison.OrdinalIgnoreCase) >= 0);
            //}

            //// Filtrar por nome do titular (se informado)
            //if (!string.IsNullOrWhiteSpace(nomeTitular))
            //{
            //    query = query.Where(p => !string.IsNullOrWhiteSpace(p.Nome) &&
            //                         p.Nome.IndexOf(nomeTitular, StringComparison.OrdinalIgnoreCase) >= 0);
            //}

            //// Filtrar por saldo (se diferente de 0)
            //if (saldo != 0)
            //{
            //    if (saldoMaiorQue)
            //        query = query.Where(p => p.Saldo >= saldo);
            //    else
            //        query = query.Where(p => p.Saldo <= saldo);
            //}

            //// Filtrar por data (se selecionada) comparando apenas a parte da data
            //if (dataSelecionada.HasValue)
            //{
            //    DateTime ds = dataSelecionada.Value.Date;
            //    if (dataMaiorQue)
            //        query = query.Where(p => p.DataDeAdesao.Date >= ds);
            //    else
            //        query = query.Where(p => p.DataDeAdesao.Date <= ds);
            //}

            //TabelaContas.ItemsSource = query.ToList();


            contaController.FiltrarContas(
                ClienteID.Text, numeroConta, numeroAgencia, tipoConta,
                nomeTitular, saldo != 0 ? saldo : (double?)null,
                dataSelecionada, saldoMaiorQue, dataMaiorQue);
        }

        private void DataMaiorQue_Click(object sender, RoutedEventArgs e)
        {
            if (dataMaiorQue)
            {
                DataMaiorQue.Content = "Menor Que";
                dataMaiorQue = false;
            }
            else
            {
                DataMaiorQue.Content = "Maior Que";
                dataMaiorQue = true;
            }
        }

        private void SaldoMaiorQue_Click(object sender, RoutedEventArgs e)
        {
            if (saldoMaiorQue)
            {
                SaldoMaiorQue.Content = "Menor Que";
                saldoMaiorQue = false;
            }
            else
            {
                SaldoMaiorQue.Content = "Maior Que";
                saldoMaiorQue = true;
            }
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)TipoOpcoes.SelectedItem;
            if (selectedItem != null)
            {
                // Salva o conteúdo (texto) na variável privada
                tipoConta = selectedItem.Content.ToString();
            }
        }

        private void NameTableButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Conta contaSelecionada = button.DataContext as Conta;
            if (contaSelecionada != null)
            {
                contaController.AbrirViewClientes(contaSelecionada);
            }
        }

        private void NumeroContaButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                contaController.AbrirViewTransacoes(button.Content.ToString());
            }
        }
    }
}
