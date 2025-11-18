using Projeto_UVV_Fintech.Banco_Dados.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_UVV_Fintech.Repository
{
    internal class TransacaoRepository
    {
        public static bool CriarTransacao(TipoTransacao tipo, double valor, int? remetenteId, int? destinatarioId, int contaId)
        {
            using var context = new DB_Context();
            Transacao novo = new Transacao();
            var contaAssociada = context.Contas.Find(contaId);
            novo.Tipo = tipo;
            novo.Valor = valor;
            novo.RemetenteId = remetenteId;
            novo.DestinatarioId = destinatarioId;
            novo.ContaId = contaId;
            novo.Conta = contaAssociada;
            contaAssociada.Transacoes.Add(novo);

            context.Transacoes.Add(novo);
            context.SaveChanges();

            return true;
        }

        public static List<Transacao> ListarTransacoes()
        {
            using var context = new DB_Context();
            return context.Transacoes.ToList();
            //foreach (var transacao in transacoes)
            //{
            //    MessageBox.Show($"ID: {transacao.Id} | Tipo: {transacao.Tipo} | Valor: {transacao.Valor}  ");

            //}
        }

        public List<Transacao> FiltrarTransacoes(int? idTransacao, int? contaRemetente, int? contaDestinatario, string? tipo, double? valor, DateTime? dataTransacao, bool? valorMaior, bool? dataMaior)
        {
            return new List<Transacao>(); // adicionar a implementação depois
        }


        public void DeletarTransacao(int transacaoId)
        {
            using var context = new DB_Context();
            var transacao = context.Transacoes.Find(transacaoId);
            if (transacao != null)
            {
                context.Transacoes.Remove(transacao);
                context.SaveChanges();


            }
            else
            {
                //MessageBox.Show("Transação não encontrada.");



            }
        }

        public void AtualizarTransacao(int transacaoId, TipoTransacao novoTipo, double novoValor)
        {
            using var context = new DB_Context();
            var transacao = context.Transacoes.Find(transacaoId);
            if (transacao != null)
            {
                transacao.Tipo = novoTipo;
                transacao.Valor = novoValor;
                context.SaveChanges();
            }
            else
            {
                //MessageBox.Show("Transação não encontrada.");


            }

        }

       public Transacao? ObterTransacaoPorId(int id)
       {
            return null; // adicionar a implementação depois
        }

       public void BuscarPorRemetente(int id)
       {

        }

       public void BuscarPorDestinatario(int id)
       {
        }
    
       public void BuscarValorMaiorQue(double valor)
       {
        
        }

       public void BuscarValorMenorQue(double valor)
       {
        }
       public void BuscarPorData(DateTime data)
       {

        }

    }

}