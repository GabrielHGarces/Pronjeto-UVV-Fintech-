using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projeto_UVV_Fintech.Model
{
    internal class ModelCliente
    {
        //Vou transformar em bool depois
            public void InserirCliente(string name, DateTime dataNascimento, string cEP, string telefone)
            {
                
                using var context = new DB_Context();
                Cliente CliNovo = new Cliente();
                CliNovo.Nome = name;
                CliNovo.Telefone = telefone;
                CliNovo.DataNascimento = dataNascimento;
                CliNovo.CEP = cEP;
                
                context.Clientes.Add(CliNovo);
                context.SaveChanges();


                
            }

            public void todosClientes()
            {
                using var context = new DB_Context();
                var clientes = context.Clientes.ToList();
                foreach (var cliente in clientes)
                {
                    MessageBox.Show($"ID: {cliente.Id}, Nome: {cliente.Nome}, Telefone: {cliente.Telefone}, Data de Nascimento: {cliente.DataNascimento}, CEP: {cliente.CEP}");
                        
                }
        }




    }
       

    
    
}
