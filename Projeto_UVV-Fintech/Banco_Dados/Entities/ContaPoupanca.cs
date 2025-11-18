using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Projeto_UVV_Fintech.Banco_Dados.Entities
{
    internal class ContaPoupanca : Conta
    {
        public const double TAXA_RENDIMENTO = 0.03; // 3% ao mês
        public const double TAXA_SAQUE = 0.01;      // 1% após 2 saques
        public const int SAQUES_GRATIS = 2;

        public int SaquesRealizadosNoMes { get; set; } = 0;
        

        // Método que aplica rendimento mensal (a cada 30 dias)
        

        // Método que desconta taxa de saque se ultrapassar os 2 gratuitos
        public void RegistrarSaque(double valor)
        {
            SaquesRealizadosNoMes++;

            if (SaquesRealizadosNoMes > SAQUES_GRATIS)
            {
                double taxa = valor * TAXA_SAQUE;
                Saldo -= valor + taxa;
            }
            else
            {
                Saldo -= valor;
            }
        }

        public override string ToString()
        {
            return base.ToString() +
                   $", Tipo: Poupança, Saques no mês: {SaquesRealizadosNoMes}: ";
        }
    }
}

