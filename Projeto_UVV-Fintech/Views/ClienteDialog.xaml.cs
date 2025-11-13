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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projeto_UVV_Fintech.Views
{
    /// <summary>
    /// Interação lógica para ClienteDialog.xam
    /// </summary>
    public partial class ClienteDialog : Window
    {
        public string NomeCliente { get; private set; }
        public string TelefoneCliente { get; private set; }
        public string CepCliente { get; private set; }

        public ClienteDialog()
        {
            InitializeComponent();
        }

        private void SomenteNumeros(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        private void SomenteLetras(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[\p{L}\s']+$");
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomeInput.Text))
            {
                MessageBox.Show("O campo Nome não pode estar vazio.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(TelefoneInput.Text))
            {
                MessageBox.Show("O campo Telefone não pode estar vazio.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(CepInput.Text))
            {
                MessageBox.Show("O campo CEP não pode estar vazio.");
                return;
            }

            NomeCliente = NomeInput.Text;
            TelefoneCliente = TelefoneInput.Text;
            CepCliente = CepInput.Text;

            DialogResult = true;
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}
