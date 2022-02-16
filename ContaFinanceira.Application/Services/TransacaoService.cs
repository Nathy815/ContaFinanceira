using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private readonly ILogger<TransacaoService> _logger;
        private readonly IEmailService _emailService;

        public TransacaoService(ITransacaoRepository transacaoRepository,
                                ILogger<TransacaoService> logger,
                                IEmailService emailService)
        {
            _transacaoRepository = transacaoRepository;
            _logger = logger;
            _emailService = emailService;
        }
             
        public async Task<List<TransacaoResponse>> Adicionar(TransacaoRequest request)
        {
            _logger.LogInformation("Convertendo requisição {request} em Transacao...", JsonConvert.SerializeObject(request));

            var transacao = new Transacao()
            {
                ContaId = request.ContaId,
                Data = DateTime.Now,
                Valor = request.Valor
            };

            _logger.LogInformation("Enviando transação {transacao} para o banco de dados...", JsonConvert.SerializeObject(transacao));

            var result = await _transacaoRepository.Criar(transacao);

            _logger.LogInformation("Buscando todas as transações feitas pela conta Id:{id}...", request.ContaId);

            var transacoes = await _transacaoRepository.Listar(request.ContaId);

            _logger.LogInformation("Banco de dados retornou transações {transacoes}", JsonConvert.SerializeObject(transacoes));

            var response = new List<TransacaoResponse>();
            foreach (var trans in transacoes)
                response.Add(new TransacaoResponse()
                {
                    Id = trans.Id,
                    Data = trans.Data,
                    Valor = trans.Valor
                });

            await _emailService.EnviarNotificacao(result);

            _logger.LogInformation("Método Adicionar() retornando {response}", JsonConvert.SerializeObject(response));

            return response;
        }

        public bool ValidarSaldoSuficiente(int contaId, decimal valor)
        {
            _logger.LogInformation("Recuperando saldo da conta Id:{id} no banco de dados...", contaId);

            var saldo = _transacaoRepository.Saldo(contaId);

            _logger.LogInformation("Saldo {saldo} recuperado para conta Id:{id} para transação no valor {valor}", saldo, contaId, valor);

            return saldo + valor >= 0;
        }
    }
}
