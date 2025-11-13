using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_UVV_Fintech.Model.Banco_Dados.Entities
{
    internal class Conta
    {
        [Key] // Define que "Id" é a chave primária da tabela, Gerado altomaticamente pelo Migration
        public int Id { get; set; }

        [Required]
        // Tipo de conta representa se é corrente ou poupança
        public string Tipo { get; set; } = string.Empty;

        [Required]
        public double Saldo { get; set; }


        // 1:N Recebe a PK do Cliente
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Criação Conta")]
        public DateTime DataCriacao { get; set; }

        // 👇 Propriedade de navegação (referência à Categoria)
        [Required]
        public  Cliente Cliente { get; set; } = null!;

        //Uma conta pode ter uma ou várias transações
        public ICollection<Transacao> Transacoes { get; set; } = [];

    }
}
