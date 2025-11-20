using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Projeto_UVV_Fintech.Banco_Dados.Entities.Conta;
using static Projeto_UVV_Fintech.Repository.ContaPoupancaRepository;
using static Projeto_UVV_Fintech.Repository.ContaCorrenteRepository;
using static Projeto_UVV_Fintech.Repository.ClienteRepository;

namespace Projeto_UVV_Fintech.Views


{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            //DeletarContaPoupanca(1);
            //DeletarContaCorrente(1);
            //DeletarCliente(1);
        }

        private void CenterWindowOnScreen()
        {
            // Pega as dimensões da tela principal em que a janela está
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            // Pega as dimensões atuais da sua janela
            double windowWidth = this.Width;
            double windowHeight = this.Height;

            // Calcula a nova posição X e Y para centralizar
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2) -25;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ViewTransacoes janelaTransacoes = new();
            janelaTransacoes.ShowDialog();
            CenterWindowOnScreen();
            this.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ViewContas janelaContas = new();
            janelaContas.ShowDialog();
            CenterWindowOnScreen();
            this.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ViewClientes janelaContas = new();
            janelaContas.ShowDialog();
            CenterWindowOnScreen();
            this.Show();
        }
    }
}