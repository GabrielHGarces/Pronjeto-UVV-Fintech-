using Projeto_UVV_Fintech.Controller;
using Projeto_UVV_Fintech.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Projeto_UVV_Fintech.Banco_Dados.Entities;

namespace Projeto_UVV_Fintech.Views
{
    public partial class ViewClientes : Window
    {
        // Filters
        public int? ClientID { get; set; } = null;
        public string? Telefone { get; set; } = "";
        public string? Cep { get; set; } = "";
        public string? ClientName { get; set; } = "";
        public int? NumeroContas { get; set; } = null;
        public DateTime? DataAdesao { get; set; } = null;
        public bool MaiorQueData { get; set; } = true;

        public ClienteController clienteController;


        public ViewClientes()
        {
            InitializeComponent();
            clienteController = new ClienteController(this);
            TabelaClientes.ItemsSource = clienteController.ListarClientes();
        }

        public ViewClientes(int IdCliente)
        {
            InitializeComponent();
            clienteController = new ClienteController(this);
            TabelaClientes.ItemsSource = clienteController.ListarClientes();

            ClientID = IdCliente;
            IdInput.Text = IdCliente.ToString();
            SearchButton_Click_1(null, null);
        }

        private void NumericTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
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
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void NovoClienteButton_Click(object sender, RoutedEventArgs e)
        {
            clienteController.CriarCliente();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void IdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ClientID = int.TryParse(IdInput.Text, out int id) ? id : null;
        }

        private void TelefoneInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Telefone = TelefoneInput.Text;
        }

        private void CEPInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Cep = CEPInput.Text;
        }

        private void NomeCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            ClientName = NomeCliente.Text;
        }

        private void NContasInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumeroContas = int.TryParse(NContasInput.Text, out int nContas) ? nContas : null;
        }

        private void DataMaiorQue_Click(object sender, RoutedEventArgs e)
        {
            if (MaiorQueData)
            {
                MaiorQueData = false;
                DataMaiorQue.Content = "Menor que";
            }
            else
            {
                MaiorQueData = true;
                DataMaiorQue.Content = "Maior que";
            }
        }

        private void DataInput_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DataAdesao = DataInput.SelectedDate;
        }

        private void SearchButton_Click_1(object sender, RoutedEventArgs e)
        {
            clienteController.FiltrarClientes(
                ClientID, Telefone, Cep, ClientName,
                NumeroContas, DataAdesao, MaiorQueData);
        }

        private void NumeroDeContas_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            Cliente clienteSelecionado = button.DataContext as Cliente;
            if (clienteSelecionado != null)
            {
                clienteController.AbrirViewContas(clienteSelecionado);
            }
        }
    }
}
