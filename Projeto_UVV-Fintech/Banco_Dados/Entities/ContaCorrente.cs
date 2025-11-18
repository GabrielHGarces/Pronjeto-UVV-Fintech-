using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;

namespace Projeto_UVV_Fintech.Banco_Dados.Entities
{
    internal class ContaCorrente : Conta
    {
        // Taxa de manutenção mensal fixa
        public const double TAXA_MANUTENCAO = 4.0;

        // Última data em que a taxa foi cobrada
        

        public override string ToString()
        {
            return base.ToString() + $", Tipo: Corrente ,Taxa de Manutenção: {TAXA_MANUTENCAO}";
        }
    }
}

