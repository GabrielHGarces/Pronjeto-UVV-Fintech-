using Projeto_UVV_Fintech.Controller;
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
        private readonly ClienteController _controller;

        public ClienteDialog(ClienteController controller)
        {
            InitializeComponent();
            _controller = controller;
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
            if (_controller.SalvarCliente(NomeInput.Text, TelefoneInput.Text, CepInput.Text))
            {
                DialogResult = true;
                Close();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}