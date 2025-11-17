using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_UVV_Fintech.Model
{
    internal class ModelContaPoupanca 
    {
        public void InserirPoupanca(double saldo, int clienteId)
        {
            using var context = new DB_Context();
            Conta novo = new ContaPoupanca();
            var clienteAssociado = context.Clientes.Find(clienteId);
            novo.Saldo = saldo;
            novo.ClienteId = clienteId;
            novo.Cliente = clienteAssociado;

            context.Contas.Add(novo);
            context.SaveChanges();


        }
    }
}
