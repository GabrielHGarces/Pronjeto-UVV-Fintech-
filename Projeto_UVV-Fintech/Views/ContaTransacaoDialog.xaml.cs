using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;

namespace Projeto_UVV_Fintech.Views
{
    public partial class ContaTransacaoDialog : Window
    {
        public int NumConta { get; set; }
        public List<Conta> contas { get; set; }

        public ContaTransacaoDialog()
        {
            contas = new List<Conta>();
            InitializeComponent();
        }

        public ContaTransacaoDialog(int NumConta)
        {
            InitializeComponent();
            this.NumConta = NumConta;
            contas = new List<Conta>
            {
                new Conta { Agencia = 1234, NumConta = 56789, Tipo = "CC", DataDeAdesao = DateTime.Now.AddMonths(-2), Saldo = 182763, Nome = "Irineu"},
                new Conta { Agencia = 2345, NumConta = 67890, Tipo = "CP", DataDeAdesao = DateTime.Now.AddMonths(-1), Saldo = 182763, Nome = "Irineu" },
                new Conta { Agencia = 3456, NumConta = 78901, Tipo = "CP", DataDeAdesao = DateTime.Now, Saldo = 182763, Nome = "Irineu" },
            };

            SelecionarCLiente(NumConta);
        }

        public class Conta
        {
            public int NumConta { get; set; }
            public int Agencia { get; set; }
            public string Tipo { get; set; } = "";
            public DateTime DataDeAdesao { get; set; }
            public int Saldo { get; set; }
            public string Nome { get; set; } = "";
        }

        private void SelecionarCLiente(int NumConta)
        {
            var conta = contas.FirstOrDefault(c => c.NumConta == NumConta);
            if (conta == null) return;

            contaInput.Text = conta.NumConta.ToString();
            TipoInput.Text = conta.Tipo;
            // Formata como moeda BR: "R$ 1.234,56"
            SaldoInput.Text = conta.Saldo.ToString("C", new CultureInfo("pt-BR"));
            DataDeCriacaoInput.Text = conta.DataDeAdesao.ToString(new CultureInfo("pt-BR"));
            NunAgenciaInput.Text = conta.Agencia.ToString();
            NomeCliente.Text = conta.Nome;
        }

        private void SomenteNumeros(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        private void SomenteLetras(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[\p{L}\s']+$");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
