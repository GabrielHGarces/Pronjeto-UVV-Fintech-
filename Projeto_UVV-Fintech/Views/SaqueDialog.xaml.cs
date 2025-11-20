using Projeto_UVV_Fintech.Controller;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projeto_UVV_Fintech.Views
{
    public partial class SaqueDialog : Window
    {
        private readonly ContaController _controller;
        private readonly int _numConta;
        private readonly string _tipoConta;

        public SaqueDialog(ContaController controller, int numConta, string tipoConta)
        {
            InitializeComponent();
            _controller = controller;
            _numConta = numConta;
            _tipoConta = tipoConta;
            ValorSaqueTextBox.Focus();
        }

        private void ConfirmarButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ValorSaqueTextBox.Text, out double valor))
            {
                _controller.Sacar(_numConta, _tipoConta, valor);
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