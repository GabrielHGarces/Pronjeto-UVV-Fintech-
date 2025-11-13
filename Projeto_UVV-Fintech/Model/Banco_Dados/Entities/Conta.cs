using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
/* 
 * Para Representar Conta corrente e poupança, Criei essa classe abstrata que as outras vão herdar
 * Assim permitindo a especificação e limitações de cada classe
 * 
 */




namespace Projeto_UVV_Fintech.Model.Banco_Dados.Entities
{
    internal abstract class Conta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double Saldo { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Criação da Conta")]
        public DateTime DataCriacao { get; set; }

        // 🔹 FK — Uma conta pertence a um cliente
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        // 🔹 Relação — Uma conta tem várias transações
        public ICollection<Transacao> Transacoes { get; set; } = [];

        public override string ToString()
        {
            return $"Id: {Id}, Saldo: {Saldo:C}, ClienteId: {ClienteId}";
        }
    }
}
