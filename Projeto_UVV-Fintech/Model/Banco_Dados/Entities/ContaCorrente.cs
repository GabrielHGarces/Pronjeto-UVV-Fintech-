using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;

namespace Projeto_UVV_Fintech.Model.Banco_Dados.Entities
{
    internal class ContaCorrente : Conta
    {
        // Taxa de manutenção mensal fixa
        public const double TAXA_MANUTENCAO = 4.0;

        // Última data em que a taxa foi cobrada
        public DateTime UltimaCobranca { get; set; }

        public void CobrarTaxaMensal()
        {
            var agora = DateTime.Now;

            // Se já passou 30 dias desde a última cobrança
            if ((agora - UltimaCobranca).TotalDays >= 30)
            {
                Saldo -= TAXA_MANUTENCAO;
                UltimaCobranca = agora;
            }
        }

        public override string ToString()
        {
            return base.ToString() + $", Tipo: Corrente, Última cobrança: {UltimaCobranca:d}";
        }
    }
}

