using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using ContaFinanceira.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Application.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository _contaRepository;

        public ContaService(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<ContaResponse> Criar(ContaRequest request)
        {
            var transacoes = new List<Transacao>();
            if (request.DepositoInicial.HasValue && request.DepositoInicial.Value > 0)
                transacoes.Add(new Transacao()
                {
                    Valor = request.DepositoInicial.Value
                });

            var conta = new Conta()
            {
                Cliente = new Cliente()
                {
                    Nome = request.NomeCliente
                },
                AgenciaId = request.AgenciaId,
                Senha = CriptografiaUtil.CriptografarSenha(request.Senha),
                DataCriacao = DateTime.Now,
                Transacoes = transacoes
            };

            conta = await _contaRepository.Criar(conta);

            return new ContaResponse()
            {
                Id = conta.Id,
                AgenciaId = conta.AgenciaId,
                ClienteNome = conta.Cliente.Nome
            };
        }
    }
}
