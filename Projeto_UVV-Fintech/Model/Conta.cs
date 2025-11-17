using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_UVV_Fintech.Model
{
    internal class Conta
    {
        public int Id { get; set; }
        public double Saldo { get; set; }
        public DateTime DataCriacao { get; set; }
        public int ClienteId { get; set; } // Chave estrangeira para o Cliente
        public ICollection<Transacao> Transacoes { get; set; }


        public Conta()
        {
            Saldo = 0;
        }
    }
}
