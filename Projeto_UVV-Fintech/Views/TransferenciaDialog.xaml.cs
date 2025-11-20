using Projeto_UVV_Fintech.Controller;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Projeto_UVV_Fintech.Views
{
    public partial class TransferenciaDialog : Window
    {
        private readonly ContaController _controller;
        private readonly int _numContaOrigem;
        private readonly string _tipoContaOrigem;

        public TransferenciaDialog(ContaController controller, int numContaOrigem, string tipoContaOrigem)
        {
            InitializeComponent();
            _controller = controller;
            _numContaOrigem = numContaOrigem;
            _tipoContaOrigem = tipoContaOrigem;
            ContaDestinoTextBox.Focus();
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

        private void ConfirmarButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(ContaDestinoTextBox.Text, out int numContaDestino))
            {
                MessageBox.Show("Número da conta de destino inválido.");
                return;
            }

            if (double.TryParse(ValorTransferenciaTextBox.Text, out double valor))
            {
                _controller.Transferir(_numContaOrigem, _tipoContaOrigem, numContaDestino, valor);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Valor inválido. Por favor, insira um número.");
            }
        }
    }
}