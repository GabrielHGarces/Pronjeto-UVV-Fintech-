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
    public partial class ContaDialog : Window
    {
        public int IdCliente { get; private set; }
        public string tipoConta = "";

        public ContaDialog()
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

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)TipoOpcoes.SelectedItem;
            if (selectedItem != null)
            {
                // Salva o conteúdo (texto) na variável privada
                tipoConta = selectedItem.Content.ToString();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Criar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ClienteID.Text))
            {
                MessageBox.Show("O campo ID não pode estar vazio.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(tipoConta))
            {
                MessageBox.Show("O campo Tipo de Conta não pode estar vazio.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(NomeCliente.Text))
            {
                MessageBox.Show("Adicione o id correto do Cliente.");
                return;
            }

            IdCliente = int.Parse(ClienteID.Text);

            DialogResult = true;
            Close();
        }

        private void Pesquisar_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;

            var dialog = new ViewClientes { Owner = this };
            dialog.ShowDialog();

            this.Opacity = 1;
        }

        private void ClienteID_TextChanged(object sender, TextChangedEventArgs e)
        {
            IdCliente = ClienteID.Text != "" ? int.Parse(ClienteID.Text) : 0;
            NomeCliente.Text = "Gatinho " + IdCliente;
        }
    }
}
