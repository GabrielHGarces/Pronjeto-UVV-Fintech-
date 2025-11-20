using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_UVV_Fintech.Banco_Dados.Entities
{
    public class Cliente
    {
        [Key] // Define que "Id" é a chave primária da tabela, Gerado altomaticamente pelo Migration
        public int Id { get; set; }

        [Required] // Campo obrigatório
        [MaxLength(100)] // Limita o tamanho máximo do texto a 100 caracteres
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        [DataType(DataType.Date)] // Informa ao ASP.NET que este campo é uma data
        [Display(Name = "Data de Adesao")] // Nome que aparece nos formulários
        public DateTime DataAdesao { get; set; } 

        [Required]
        [MaxLength(8)]//Tamanho Maximo
        public string CEP { get; set; } = null!;

        [Required]
        [MaxLength(9)] //Tamanho Maximo
        public string Telefone { get; set; } = null!;
        public int QuantidadeContas { get; set; } = 0;




        //Um CLiente pode ter várias contas
        public ICollection<Conta> Contas { get; set; } = [];
        
        public Cliente() { }
    }
}
