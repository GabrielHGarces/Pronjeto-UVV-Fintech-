using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_UVV_Fintech.Repository
{
    internal class ClienteRepository
    {
        //Vou transformar em bool depois
        public static bool CriarCliente(string name, DateTime dataNascimento, string cEP, string telefone)
        {

            using var context = new DB_Context();
            Cliente CliNovo = new Cliente();
            CliNovo.Nome = name;
            CliNovo.Telefone = telefone;
            CliNovo.DataNascimento = dataNascimento;
            CliNovo.CEP = cEP;

            context.Clientes.Add(CliNovo);
            context.SaveChanges();

            return true;
        }

        public static List<Cliente> ListarClientes()
        {
            using var context = new DB_Context();
            return context.Clientes.ToList();
            //foreach (var cliente in clientes)
            //{
            //    MessageBox.Show($"ID: {cliente.Id}, Nome: {cliente.Nome}, Telefone: {cliente.Telefone}, Data de Nascimento: {cliente.DataNascimento}, CEP: {cliente.CEP}");

            //}
        }

        public static List<Cliente> FiltrarClientes(int? idCliente, string? telefone, string? cep, string? nomeCliente, int? numeroDeContas, DateTime? dataAdesao, bool? dataMaiorQue)
        {
            return new List<Cliente>(); // adicionar a implementação depois
        }


        public void DeletarCliente(int clienteId)
        {
            using var context = new DB_Context();
            var cliente = context.Clientes.Find(clienteId);
            if (cliente != null)
            {
                context.Clientes.Remove(cliente);
                context.SaveChanges();
            }
            else
            {
                //MessageBox.Show("Cliente não encontrado.");
            }
        }

        public void AtualizarCliente(int clienteId, string novoNome, DateTime novaDataNascimento, string novoCEP, string novoTelefone)
        {
            using var context = new DB_Context();
            var cliente = context.Clientes.Find(clienteId);
            if (cliente != null)
            {
                cliente.Nome = novoNome;
                cliente.DataNascimento = novaDataNascimento;
                cliente.CEP = novoCEP;
                cliente.Telefone = novoTelefone;
                context.SaveChanges();
            }
            else
            {
                //MessageBox.Show("Cliente não encontrado.");
            }
        }

        public Cliente ObterClientePorId(int clienteId)
        {
            using var context = new DB_Context();
            return context.Clientes.Find(clienteId);
        }

        public List<Cliente> ObterTodosClientes()
        {
            using var context = new DB_Context();
            return context.Clientes.ToList();
        }

        public void BuscarPorId(int id)
        {

            using var context = new DB_Context();
            var cliente = context.Clientes.Find(id);
            if (cliente != null)
            {
                //MessageBox.Show($"ID: {cliente.Id}, Nome: {cliente.Nome}, Telefone: {cliente.Telefone}, Data de Nascimento: {cliente.DataNascimento}, CEP: {cliente.CEP}");
            }
            else
            {
                //MessageBox.Show("Cliente não encontrado.");
            }

        }

        public void BuscarPorNome(string nome)
        {
            using var context = new DB_Context();
            var clientes = context.Clientes
                .Where(c => c.Nome.Contains(nome))
                .ToList();
            if (clientes.Any())
            {
                foreach (var cliente in clientes)
                {
                    //MessageBox.Show($"ID: {cliente.Id}, Nome: {cliente.Nome}, Telefone: {cliente.Telefone}, Data de Nascimento: {cliente.DataNascimento}, CEP: {cliente.CEP}");
                }
            }
            else
            {
                //MessageBox.Show("Nenhum cliente encontrado com esse nome.");
            }
        }

        public void BuscarPorTelefone(string telefone)
        {
            using var context = new DB_Context();
            var clientes = context.Clientes
                .Where(c => c.Telefone.Contains(telefone))
                .ToList();
            if (clientes.Any())
            {
                foreach (var cliente in clientes)
                {
                    //MessageBox.Show($"ID: {cliente.Id}, Nome: {cliente.Nome}, Telefone: {cliente.Telefone}, Data de Nascimento: {cliente.DataNascimento}, CEP: {cliente.CEP}");
                }
            }
            else
            {
                //MessageBox.Show("Nenhum cliente encontrado com esse telefone.");
            }
        }

        public void BuscarPorCEP(string cep)
        {
            using var context = new DB_Context();
            var clientes = context.Clientes
                .Where(c => c.CEP.Contains(cep))
                .ToList();
            if (clientes.Any())
            {
                foreach (var cliente in clientes)
                {
                    //MessageBox.Show($"ID: {cliente.Id}, Nome: {cliente.Nome}, Telefone: {cliente.Telefone}, Data de Nascimento: {cliente.DataNascimento}, CEP: {cliente.CEP}");
                }
            }
            else
            {
                //MessageBox.Show("Nenhum cliente encontrado com esse CEP.");
            }
        }

        public void BuscarPorDataNascimento(DateTime dataNascimento)
        {
            using var context = new DB_Context();
            var clientes = context.Clientes
                .Where(c => c.DataNascimento == dataNascimento)
                .ToList();
            if (clientes.Any())
            {
                foreach (var cliente in clientes)
                {
                    //MessageBox.Show($"ID: {cliente.Id}, Nome: {cliente.Nome}, Telefone: {cliente.Telefone}, Data de Nascimento: {cliente.DataNascimento}, CEP: {cliente.CEP}");
                }
            }
            else
            {
                //MessageBox.Show("Nenhum cliente encontrado com essa data de nascimento.");
            }
        }

        

    }
    
}
