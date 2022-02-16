using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContaFinanceira.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly ITransacaoRepository _transacaoRepository;
        private readonly IConfiguration _configuration;

        public EmailService(ILogger<EmailService> logger,
                            ITransacaoRepository transacaoRepository,
                            IConfiguration configuration)
        {
            _logger = logger;
            _transacaoRepository = transacaoRepository;
            _configuration = configuration;
        }

        public async Task EnviarNotificacao(Transacao transacao)
        {
            _logger.LogInformation("Preparando transação Id:{id} para envio..", transacao.Id);

            var model = new EmailRequest()
            {
                Id = transacao.Id,
                Data = transacao.Data,
                Valor = transacao.Valor,
                Cliente = new EmailClienteRequest()
                {
                    Nome = transacao.Conta.Cliente.Nome,
                    Email = transacao.Conta.Cliente.Email
                },
                Conta = new EmailContaRequest()
                {
                    Id = transacao.ContaId,
                    Saldo = _transacaoRepository.Saldo(transacao.ContaId)
                }
            };

            var request = JsonConvert.SerializeObject(model);

            var emailServer = $"{_configuration.GetSection("EmailAPI").Value}/api/notificacoes";

            _logger.LogInformation("Enviando requisição {requisicao} para URL {url}", request, emailServer);

            var _client = new HttpClient();
            var result = await _client.PostAsync(emailServer,
                                                 new StringContent(
                                                     request, 
                                                     Encoding.UTF8, 
                                                     "application/json"));

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _logger.LogInformation("Notificação enviada com sucesso!");

                await _transacaoRepository.SetNotificacaoEnviada(transacao.Id);

                _logger.LogInformation("Transação Id:{id} atualizada com sucesso.", transacao.Id);
            }
            else
                _logger.LogError("Erro ao enviar notificação. Detalhes: {ex}", JsonConvert.SerializeObject(result));
        }
    }
}
