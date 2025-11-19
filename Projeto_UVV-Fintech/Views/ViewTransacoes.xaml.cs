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
using System.Globalization;
using Projeto_UVV_Fintech.Banco_Dados.Entities;
using Projeto_UVV_Fintech.Controller;

namespace Projeto_UVV_Fintech.Views
{
    public partial class ViewTransacoes : Window
    {
        public bool valorMaiorQue = false;
        public bool dataMaiorQue = true;
        public int? ID;
        public double valor = 0;
        public string tipoTransacao = "Todos";
        public int? contaRemetente = null;
        public int? contaDestinatario = null;
        public DateTime? dataSelecionada = null;

        private readonly TransacaoController _controller;

        public ViewTransacoes()
        {
            InitializeComponent();
            _controller = new TransacaoController(this);
            _controller.ListarTransacoes();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ValorMaiorQue_Click(object sender, RoutedEventArgs e)
        {
            if (valorMaiorQue)
            {
                ValorMaiorQue.Content = "Menor Que";
                valorMaiorQue = false;
            }
            else
            {
                ValorMaiorQue.Content = "Maior Que";
                valorMaiorQue = true;
            }
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

        private void NumericTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NumericTextBox.Text = "";
        }

        private void NumericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(NumericTextBox.Text, out valor))
            {
                valor = double.Parse(NumericTextBox.Text);
            }
            else
            {
                Console.WriteLine("Erro ao converter para decimal!!!");
            }

            string valorFormatado = String.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valor);
            Console.WriteLine(valorFormatado);

            NumericTextBox.Text = valorFormatado;
            if (valor == 0)
            {
                NumericTextBox.Text = "";
            }
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)TipoOpcoes.SelectedItem;
            if (selectedItem != null)
            {
                tipoTransacao = selectedItem.Content.ToString();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            _controller.FiltrarTransacoes(ID, contaRemetente, contaDestinatario, tipoTransacao, valor != 0 ? valor : null, dataSelecionada, valorMaiorQue, dataMaiorQue);
        }

        private void IdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            ID = int.TryParse(IdInput.Text, out int id) ? id : (int?)null;
        }

        private void RemetenteInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            contaRemetente = int.TryParse(RemetenteInput.Text, out int remetente) ? remetente : (int?)null;
        }

        private void DestinatarioInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            contaDestinatario = int.TryParse(DestinatarioInput.Text, out int destinatario) ? destinatario : (int?)null;
        }

        private void DataInput_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dataSelecionada = DataInput.SelectedDate;
        }

        private void ContaRemetente_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is Transacao transacaoSelecionada)
            {
                //_controller.AbrirViewContas(transacaoSelecionada.ContaRemetente);
            }
        }

        private void ContaDestinatario_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button?.DataContext is Transacao transacaoSelecionada)
            {
                //_controller.AbrirViewContas(transacaoSelecionada.ContaDestinatario);
            }
        }
    }
}
