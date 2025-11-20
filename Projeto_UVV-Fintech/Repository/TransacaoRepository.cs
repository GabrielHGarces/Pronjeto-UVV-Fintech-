using Microsoft.EntityFrameworkCore;
﻿using Projeto_UVV_Fintech.Banco_Dados.Entities;
﻿using Projeto_UVV_Fintech.Repository.Interfaces;
﻿using System;
﻿using System.Collections.Generic;
﻿using System.Linq;
﻿using System.Text;
﻿using System.Threading.Tasks;
﻿
﻿/*
﻿ * ====================================================================
﻿ * APLICAÇÃO DE BOAS PRÁTICAS: Implementação do Contrato
﻿ * ====================================================================
﻿ *
﻿ * 1. CUMPRINDO O CONTRATO:
﻿ *    - Esta classe implementa `ITransacaoRepository` para os métodos
﻿ *      de listagem e filtro, permitindo o desacoplamento.
﻿ *
﻿ * 2. ESCOLHA PRÁTICA - MÉTODO ESTÁTICO:
﻿ *    - O método `CriarTransacao` foi mantido como `static` por uma
﻿ *      questão de simplicidade.
﻿ *    - Como ele é chamado por outros repositórios (ContaCorrente e
﻿ *      ContaPoupanca), a alternativa seria injetar o repositório de
﻿ *      transação dentro dos outros, o que aumentaria a complexidade.
﻿ *    - Manter este método como `static` foi uma decisão pragmática
﻿ *      para não complicar demais a arquitetura neste momento.
﻿ *
﻿ */
﻿namespace Projeto_UVV_Fintech.Repository
﻿{
﻿    public class TransacaoRepository : ITransacaoRepository
﻿    {
﻿        public static bool CriarTransacao(
﻿            TipoTransacao tipo,
﻿            double valor,
﻿            int? contaRemetenteId,
﻿            int? contaDestinatarioId)
﻿        {
﻿            using var context = new DB_Context();
﻿
﻿            Conta contaRemetente = null;
﻿            Conta contaDestinatario = null;
﻿
﻿            if (contaRemetenteId != null)
﻿                contaRemetente = context.Contas.FirstOrDefault(c => c.Id == contaRemetenteId);
﻿
﻿            if (contaDestinatarioId != null)
﻿                contaDestinatario = context.Contas.FirstOrDefault(c => c.Id == contaDestinatarioId);
﻿
﻿            // Criar entidade final
﻿            var novaTransacao = new Transacao
﻿            {
﻿                Tipo = tipo,
﻿                Valor = valor,
﻿                ContaRemetenteId = contaRemetenteId,
﻿                ContaDestinatarioId = contaDestinatarioId,
﻿                ContaRemetente = contaRemetente,
﻿                ContaDestinatario = contaDestinatario,
﻿                DataHoraTransacao = DateTime.Now
﻿            };
﻿
﻿            context.Transacoes.Add(novaTransacao);
﻿            context.SaveChanges();
﻿            return true;
﻿        }
﻿
﻿        public List<Transacao> ListarTransacoes()
﻿        {
﻿            using var context = new DB_Context();
﻿
﻿            return context.Transacoes
﻿                .Include(t => t.ContaRemetente)
﻿                .Include(t => t.ContaDestinatario)
﻿                .ToList();
﻿        }
﻿
﻿        public List<Transacao> FiltrarTransacoes(
﻿            int? idTransacao,
﻿            int? contaRemetente,
﻿            int? contaDestinatario,
﻿            string? tipo,
﻿            double? valor,
﻿            DateTime? dataTransacao,
﻿            bool? valorMaior,
﻿            bool? dataMaior)
﻿        {
﻿            var transacoes = ListarTransacoes();
﻿
﻿            if (tipo == "Depósito")
﻿            {
﻿                tipo = "Deposito";
﻿            } 
﻿            else if (tipo == "Transferência")
﻿            {
﻿                tipo = "Transferencia";
﻿            }
﻿
﻿                        var filtrado = transacoes
﻿                            .Where(t =>
﻿                                (idTransacao == null || t.Id == idTransacao) &&
﻿                                (contaRemetente == null || (t.ContaRemetente != null && t.ContaRemetente.NumeroConta == contaRemetente)) &&
﻿                                (contaDestinatario == null || (t.ContaDestinatario != null && t.ContaDestinatario.NumeroConta == contaDestinatario)) &&
﻿                                (string.IsNullOrWhiteSpace(tipo) || tipo == "Todos" || t.Tipo.ToString().Contains(tipo, StringComparison.OrdinalIgnoreCase)) &&﻿                    (
﻿                        valor == null ||
﻿                        (
﻿                            valorMaior == true ? t.Valor >= valor :
﻿                            valorMaior == false ? t.Valor <= valor :
﻿                            true
﻿                        )
﻿                    ) &&
﻿                    (
﻿                        dataTransacao == null ||
﻿                        (
﻿                            dataMaior == true ? t.DataHoraTransacao >= dataTransacao :
﻿                            dataMaior == false ? t.DataHoraTransacao <= dataTransacao :
﻿                            true
﻿                        )
﻿                    )
﻿                )
﻿                .ToList();
﻿
﻿            return filtrado;
﻿        }
﻿    }
﻿}
﻿