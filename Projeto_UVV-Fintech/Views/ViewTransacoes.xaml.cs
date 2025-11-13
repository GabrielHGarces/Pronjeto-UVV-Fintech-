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
using System.Linq;


namespace Projeto_UVV_Fintech.Views
{
    public partial class ViewTransacoes : Window
    {
        public bool valorMaiorQue = false;
        public bool dataMaiorQue = true;
        public int? ID;
        public double valor = 0;
        public string tipoTransacao = "";
        public List<Transacao> transacoes;
        public int? contaRemetente = null;
        public int? contaDestinatario = null;
        public DateTime? dataSelecionada = null;

        public ViewTransacoes()
        {
            InitializeComponent();

            transacoes = new List<Transacao>
            {
                new Transacao { Id = 1, Valor = 500.5, Tipo = "Saque", DataEHora = DateTime.Now, ContaRemetente = 888888888, ContaDestinatario = 999999999 },
                new Transacao { Id = 2, Valor = 600.87, Tipo = "Saque", DataEHora = new(2025, 11, 12), ContaRemetente = 999999999, ContaDestinatario = 888888888 },
                new Transacao { Id = 3, Valor = 700, Tipo = "Saque", DataEHora = new(2025, 11, 13), ContaRemetente = 111111111, ContaDestinatario = 999999999 },
                new Transacao { Id = 4, Valor = 800, Tipo = "Transferência", DataEHora = new(2025, 11, 14), ContaRemetente = 999999999, ContaDestinatario = 999999999 },
                new Transacao { Id = 1, Valor = 900, Tipo = "Transferência", DataEHora = new(2025, 11, 15), ContaRemetente = 999999999, ContaDestinatario = 999999999 },
                new Transacao { Id = 1, Valor = 1000, Tipo = "Transferência", DataEHora = new(2025, 11, 16), ContaRemetente = 999999999, ContaDestinatario = 999999999 },
                new Transacao { Id = 1, Valor = 500, Tipo = "Depósito", DataEHora = new(2025, 11, 11), ContaRemetente = 999999999, ContaDestinatario = 999999999 },
                new Transacao { Id = 1, Valor = 500, Tipo = "Depósito", DataEHora = new(2025, 11, 11), ContaRemetente = 999999999, ContaDestinatario = 999999999 },
                new Transacao { Id = 1, Valor = 500, Tipo = "Depósito", DataEHora = new(2025, 11, 11), ContaRemetente = 999999999, ContaDestinatario = 999999999 },
                new Transacao { Id = 1, Valor = 500, Tipo = "Depósito", DataEHora = new(2025, 11, 11), ContaRemetente = 999999999, ContaDestinatario = 999999999 },
            };


            // Atribui os dados à tabela
            TabelaTransacoes.ItemsSource = transacoes;

        }

        public class Transacao
        {
            public int Id { get; set; }
            public double Valor { get; set; }
            public string Tipo { get; set; } = "";
            public DateTime DataEHora { get; set; }
            public int ContaRemetente { get; set; }
            public int ContaDestinatario { get; set; }
        }


        private void NumericTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Obtém o TextBox que disparou o evento
            TextBox textBox = sender as TextBox;

            // Verifica se o caractere inserido é um dígito (0-9)
            bool isDigit = Char.IsDigit(e.Text, 0);

            // Verifica se o caractere é um separador decimal (vírgula ou ponto, dependendo da cultura)
            // Usamos CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator para ser compatível com a região.
            string decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;

            // Para o Brasil, o separador é a vírgula. Vamos permitir explicitamente ponto e vírgula.
            bool isSeparator = e.Text == ",";

            if (isDigit)
            {
                // Aceita dígitos
                e.Handled = false;
            }
            else if (isSeparator)
            {
                // Aceita separadores APENAS se ainda não houver um no texto
                if (textBox.Text.Contains(","))
                {
                    e.Handled = true; // Rejeita se já houver um separador
                }
                else
                {
                    e.Handled = false; // Aceita se for o primeiro separador
                }
            }
            else
            {
                // Rejeita qualquer outro caractere (letras, símbolos, espaços)
                e.Handled = true;
            }
        }

        // Opcional, mas recomendado: impede que o usuário cole texto não numérico
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

            // Formata o valor como moeda brasileira (R$)
            string valorFormatado = String.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valor);

            // Exibe o resultado: R$ 123.456,78
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
                // Salva o conteúdo (texto) na variável privada
                tipoTransacao = selectedItem.Content.ToString();
            }

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var filtrado = transacoes
                .Where(p => (p.Id == ID || ID == null) &&
                (p.Tipo.Contains(tipoTransacao) || tipoTransacao == "Todos" || tipoTransacao == null) &&
                (p.ContaRemetente.ToString().Contains(contaRemetente.ToString()) || contaRemetente == null) &&
                (p.ContaDestinatario.ToString().Contains(contaDestinatario.ToString()) || contaDestinatario == null) &&
                ((valorMaiorQue? p.Valor >= valor : p.Valor <= valor) || valor == 0) &&
                ((dataMaiorQue ? p.DataEHora >= dataSelecionada : p.DataEHora <= dataSelecionada) || dataSelecionada == null))
                .ToList();

            TabelaTransacoes.ItemsSource = filtrado;
        }

        private void IdInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (IdInput.Text == "")
                {
                    ID = null;
                }
                else
                {
                    ID = int.Parse(IdInput.Text);
                }
            }
            catch (Exception)
            {
                ID = -1;
            }
        }

        private void RemetenteInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RemetenteInput.Text != "")
            {
                try
                {
                    contaRemetente = int.Parse(RemetenteInput.Text);
                }
                catch (Exception)
                {
                    contaRemetente = null;
                }
            }
            else
            {
                contaRemetente = null;
            }
        }

        private void DestinatarioInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DestinatarioInput.Text != "")
            {
                try
                {
                    contaDestinatario = int.Parse(DestinatarioInput.Text);
                }
                catch (Exception)
                {
                    contaDestinatario = null;
                }
            }
            else
            {
                contaDestinatario = null;
            }
        }

        private void DataInput_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dataSelecionada = DataInput.SelectedDate;
        }
    }
}
