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
using Projeto_UVV_Fintech.Controller;
using Projeto_UVV_Fintech.ViewModels;

namespace Projeto_UVV_Fintech.Views
{
    public partial class ContaTransacaoDialog : Window
    {
        public int NumConta { get; set; }
        //public List<Conta> contas { get; set; }
        ContaController _controller;

        public ContaTransacaoDialog()
        {
            //contas = new List<Conta>();
            InitializeComponent();
        }

        public ContaTransacaoDialog(int NumConta, ContaController _controller)
        {
            InitializeComponent();
            this.NumConta = NumConta;
            this._controller = _controller;
            //contas = new List<Conta>
            //{
            //    new Conta { Agencia = 1234, NumConta = 56789, Tipo = "CC", DataDeAdesao = DateTime.Now.AddMonths(-2), Saldo = 182763, Nome = "Irineu"},
            //    new Conta { Agencia = 2345, NumConta = 67890, Tipo = "CP", DataDeAdesao = DateTime.Now.AddMonths(-1), Saldo = 182763, Nome = "Irineu" },
            //    new Conta { Agencia = 3456, NumConta = 78901, Tipo = "CP", DataDeAdesao = DateTime.Now, Saldo = 182763, Nome = "Irineu" },
            //};

            SelecionarCLiente(NumConta);
        }

        //public class Conta
        //{
        //    public int NumConta { get; set; }
        //    public int Agencia { get; set; }
        //    public string Tipo { get; set; } = "";
        //    public DateTime DataDeAdesao { get; set; }
        //    public int Saldo { get; set; }
        //    public string Nome { get; set; } = "";
        //}

        private void SelecionarCLiente(int NumConta)
        {

            ContaViewModel? conta = _controller.FiltrarContas(null, NumConta, null, null, null, null, null, null, null).FirstOrDefault();
            if (conta == null) return;

            _controller.ListarContas();

            contaInput.Text = conta.NumeroConta.ToString();
            TipoInput.Text = conta.Tipo;
            // Formata como moeda BR: "R$ 1.234,56"
            SaldoInput.Text = conta.Saldo.ToString("C", new CultureInfo("pt-BR"));
            DataDeCriacaoInput.Text = conta.DataCriacao.ToString(new CultureInfo("pt-BR"));
            NunAgenciaInput.Text = conta.Agencia.ToString();
            NomeCliente.Text = conta.NomeCliente;
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

        private void SacarButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.AbrirSaqueDialog(this, NumConta, TipoInput.Text))
            {
                SelecionarCLiente(NumConta);
            }
        }

        private void DepositarButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.AbrirDepositoDialog(this, NumConta, TipoInput.Text))
            {
                SelecionarCLiente(NumConta);
            }
        }

        private void TransferirButton_Click(object sender, RoutedEventArgs e)
        {
            if (_controller.AbrirTransferenciaDialog(this, NumConta, TipoInput.Text))
            {
                SelecionarCLiente(NumConta);
            }
        }

        private void VerTransacoes_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            _controller.AbrirTransacoes(contaInput.Text);
        }
    }
}
