using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Application.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly ITransacaoRepository _transacaoRepository;

        public TransacaoService(ITransacaoRepository transacaoRepository)
        {
            _transacaoRepository = transacaoRepository;
        }
             
        public async Task<List<TransacaoResponse>> Adicionar(TransacaoRequest request)
        {
            var transacao = new Transacao()
            {
                ContaId = request.ContaId,
                Data = DateTime.Now,
                Valor = request.Valor
            };

            await _transacaoRepository.Criar(transacao);

            var transacoes = await _transacaoRepository.Listar(request.ContaId);

            var response = new List<TransacaoResponse>();
            foreach (var trans in transacoes)
                response.Add(new TransacaoResponse()
                {
                    Id = trans.Id,
                    Data = trans.Data,
                    Valor = trans.Valor
                });

            return response;
        }

        public bool ValidarSaldoSuficiente(int contaId, decimal valor)
        {
            var saldo = _transacaoRepository.Saldo(contaId);

            return saldo + valor >= 0;
        }
    }
}
