using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Projeto_UVV_Fintech.Banco_Dados.Entities
{
    public enum TipoTransacao
    {
        Saque,
        Deposito,
        Transferencia
    }
    public class Transacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TipoTransacao Tipo { get; set; } 

        [Required]
        public double Valor { get; set; }

        [Required]
        //Conta que enviou
        public int? RemetenteId { get; set; }



        [Required]
        //Conta que recebeu
        public int? DestinatarioId { get; set; }

        // Campo para armazenar a data de criação
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data/Hora Transacao")]
        public DateTime DataHoraTransacao { get; set; }





        // 1:N Recebe a PK da Conta
        [ForeignKey("Conta")]
        public int ContaId { get; set; }

        // 👇 Propriedade de navegação (referência à Categoria)
        public Conta Conta { get; set; } = null!;
        public Transacao() { }
        public Transacao( TipoTransacao tipo, double valor, int? remetenteId, int? destinatarioId, int contaId)
        {
            
            Tipo = tipo;
            Valor = valor;
            RemetenteId = remetenteId;
            DestinatarioId = destinatarioId;
            ContaId = contaId;
            
        }
    }
}
