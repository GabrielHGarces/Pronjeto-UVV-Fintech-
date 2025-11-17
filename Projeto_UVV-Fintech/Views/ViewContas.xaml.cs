using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
using static Projeto_UVV_Fintech.Views.ViewClientes;

namespace Projeto_UVV_Fintech.Views
{
    public partial class ViewContas : Window
    {
        //Filters
        public int? numeroConta = null;
        public int? numeroAgencia = null;
        public string? tipoConta = "Todos";
        public string? nomeTitular = "";
        public double saldo = 0;
        public DateTime? dataSelecionada = null;
        public bool saldoMaiorQue = true;
        public bool dataMaiorQue = true;
        public List<Conta> contas;

        public ViewContas()
        {
            InitializeComponent();

            contas = new List<Conta>
            {
                new Conta { IdCliente = 1, Agencia = 1234, NumConta = 56789, Tipo = "CC", DataDeAdesao = DateTime.Now.AddMonths(-2), Saldo = 182763, Nome = "Irineu"},
                new Conta { IdCliente = 2, Agencia = 2345, NumConta = 67890, Tipo = "CP", DataDeAdesao = DateTime.Now.AddMonths(-1), Saldo = 182763, Nome = "Irineu" },
                new Conta { IdCliente = 3, Agencia = 3456, NumConta = 78901, Tipo = "CP", DataDeAdesao = DateTime.Now, Saldo = 182763, Nome = "Irineu" },
            };


            TabelaContas.ItemsSource = contas;
        }

        public ViewContas(int nConta)
        {
            InitializeComponent();

            contas = new List<Conta>
            {
                new Conta { IdCliente = 1, Agencia = 1234, NumConta = 56789, Tipo = "CC", DataDeAdesao = DateTime.Now.AddMonths(-2), Saldo = 182763, Nome = "Irineu"},
                new Conta { IdCliente = 2,  Agencia = 2345, NumConta = 67890, Tipo = "CP", DataDeAdesao = DateTime.Now.AddMonths(-1), Saldo = 182763, Nome = "Irineu" },
                new Conta { IdCliente = 3,  Agencia = 3456, NumConta = 78901, Tipo = "CP", DataDeAdesao = DateTime.Now, Saldo = 182763, Nome = "Irineu" },
            };

            NConta.Text = nConta.ToString();
            //numeroConta = nConta;


            TabelaContas.ItemsSource = contas;
            PerformSearch();
        }
        public ViewContas(int nConta, string a)
        {
            InitializeComponent();

            contas = new List<Conta>
            {
                new Conta { IdCliente = 1, Agencia = 1234, NumConta = 56789, Tipo = "CC", DataDeAdesao = DateTime.Now.AddMonths(-2), Saldo = 182763, Nome = "Irineu"},
                new Conta { IdCliente = 2,  Agencia = 2345, NumConta = 67890, Tipo = "CP", DataDeAdesao = DateTime.Now.AddMonths(-1), Saldo = 182763, Nome = "Irineu" },
                new Conta { IdCliente = 3,  Agencia = 3456, NumConta = 78901, Tipo = "CP", DataDeAdesao = DateTime.Now, Saldo = 182763, Nome = "Irineu" },
            };

            ClienteID.Text = nConta.ToString();
            //numeroConta = nConta;


            TabelaContas.ItemsSource = contas;
            PerformSearch();
        }

        public class Conta
        {
            public int IdCliente { get; set; }
            public int NumConta { get; set; }
            public int Agencia { get; set; }
            public string Tipo { get; set; } = "";
            public DateTime DataDeAdesao { get; set; }
            public int Saldo { get; set; }
            public string Nome { get; set; } = "";
        }

        private void DataInput_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dataSelecionada = DataInput.SelectedDate;
        }

        private void NumericTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SaldoD.Text = "";
        }

        private void NumericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(SaldoD.Text, out saldo))
            {
                saldo = double.Parse(SaldoD.Text);
            }
            else
            {
                Console.WriteLine("Erro ao converter para decimal!!!");
            }

            // Formata o valor como moeda brasileira (R$)
            string valorFormatado = String.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", saldo);

            // Exibe o resultado: R$ 123.456,78
            Console.WriteLine(valorFormatado);

            SaldoD.Text = valorFormatado;
            if (saldo == 0)
            {
                SaldoD.Text = "";
            }
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

        private void AdicionarConta_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;

            var dialog = new ContaDialog { Owner = this };
            bool? resultado = dialog.ShowDialog();

            this.Opacity = 1;

            if (resultado == true)
            {
                //Chamar ContaController.CriarConta();
                int IdCliente = dialog.IdCliente;
                string tipoConta = dialog.tipoConta;

                MessageBox.Show($"Conta criada:\nId Cliente: {IdCliente}\nTipo Conta: {tipoConta}");
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NAgencia_TextChanged(object sender, TextChangedEventArgs e)
        {
            numeroAgencia = int.TryParse(NAgencia.Text, out int NumAgencia) ? NumAgencia : null;
        }

        private void NConta_TextChanged(object sender, TextChangedEventArgs e)
        {
            numeroConta = int.TryParse(NConta.Text, out int nContas) ? nContas : null;
        }

        private void NomeCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            nomeTitular = NomeCliente.Text;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            if (contas == null)
            {
                TabelaContas.ItemsSource = null;
                return;
            }

            var query = contas.AsEnumerable();

            // Filtrar por ID do cliente (se informado)
            if (!string.IsNullOrWhiteSpace(ClienteID.Text))
            {
                string buscaID = ClienteID.Text;
                query = query.Where(p => p.IdCliente.ToString().Contains(buscaID));
            }

            // Filtrar por número da conta (se informado)
            if (numeroConta.HasValue)
            {
                string buscaConta = numeroConta.Value.ToString();
                query = query.Where(p => p.NumConta.ToString().Contains(buscaConta));
            }

            // Filtrar por número da agência (se informado)
            if (numeroAgencia.HasValue)
            {
                string buscaAgencia = numeroAgencia.Value.ToString();
                query = query.Where(p => p.Agencia.ToString().Contains(buscaAgencia));
            }

            // Filtrar por tipo de conta (se não for "Todos" e não for nulo/vazio)
            if (!string.IsNullOrWhiteSpace(tipoConta) && !string.Equals(tipoConta, "Todos", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(p => !string.IsNullOrWhiteSpace(p.Tipo) &&
                                     p.Tipo.IndexOf(tipoConta, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Filtrar por nome do titular (se informado)
            if (!string.IsNullOrWhiteSpace(nomeTitular))
            {
                query = query.Where(p => !string.IsNullOrWhiteSpace(p.Nome) &&
                                     p.Nome.IndexOf(nomeTitular, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Filtrar por saldo (se diferente de 0)
            if (saldo != 0)
            {
                if (saldoMaiorQue)
                    query = query.Where(p => p.Saldo >= saldo);
                else
                    query = query.Where(p => p.Saldo <= saldo);
            }

            // Filtrar por data (se selecionada) comparando apenas a parte da data
            if (dataSelecionada.HasValue)
            {
                DateTime ds = dataSelecionada.Value.Date;
                if (dataMaiorQue)
                    query = query.Where(p => p.DataDeAdesao.Date >= ds);
                else
                    query = query.Where(p => p.DataDeAdesao.Date <= ds);
            }

            TabelaContas.ItemsSource = query.ToList();
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

        private void SaldoMaiorQue_Click(object sender, RoutedEventArgs e)
        {
            if (saldoMaiorQue)
            {
                SaldoMaiorQue.Content = "Menor Que";
                saldoMaiorQue = false;
            }
            else
            {
                SaldoMaiorQue.Content = "Maior Que";
                saldoMaiorQue = true;
            }
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

        private void NameTableButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Converte o 'sender' (que é o botão clicado) para um objeto Button.
            Button button = sender as Button;
            if (button == null) return;

            // 2. Obtém o DataContext do botão, que é o objeto 'Conta' da linha clicada.
            Conta contaSelecionada = button.DataContext as Conta;
            if (contaSelecionada == null) return;

            // 3. Pega a informação que você quer passar (neste caso, o nome do cliente).
            int idCliente = contaSelecionada.IdCliente;

            // 4. Abre a janela ViewClientes, passando o nome do cliente para o construtor.
            this.Hide();
            var window = new ViewClientes(idCliente) { Owner = this };
            window.ShowDialog();
            this.Close();
        }

        private void NumeroContaButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            this.Opacity = 0.5;

            //var dialog = new ContaTransacaoDialog { Owner = this };
            //bool? resultado = dialog.ShowDialog(int.Parse(button.Content.ToString()));

            // ViewContas.xaml.cs
            var dialog = new ContaTransacaoDialog(int.Parse(button.Content.ToString())) { Owner = this };
            bool? resultado = dialog.ShowDialog();

            this.Opacity = 1;

            if (resultado == true)
            {
                //int IdCliente = dialog.IdCliente;
                //string tipoConta = dialog.tipoConta;

                //MessageBox.Show($"Conta criada:\nId Cliente: {IdCliente}\nTipo Conta: {tipoConta}");
            }
        }
    }
}
