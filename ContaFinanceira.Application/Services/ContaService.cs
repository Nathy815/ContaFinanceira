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
        private readonly ILogger<ContaService> _logger;

        public ContaService(IContaRepository contaRepository,
                            ILogger<ContaService> logger)
        {
            _contaRepository = contaRepository;
            _logger = logger;
        }

        public async Task<ContaResponse> Criar(ContaRequest request)
        {
            _logger.LogInformation("Convertendo request {request} em Conta para banco de dados...", JsonConvert.SerializeObject(request));

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
                    Nome = request.NomeCliente,
                    TipoPessoa = request.TipoPessoa,
                    CpfCnpj = request.CpfCnpj,
                    Email = request.Email
                },
                AgenciaId = request.AgenciaId,
                Senha = CriptografiaUtil.CriptografarSenha(request.Senha),
                DataCriacao = DateTime.Now,
                Transacoes = transacoes
            };

            _logger.LogInformation("Enviando conta {conta} para o banco de dados...", JsonConvert.SerializeObject(conta));

            conta = await _contaRepository.Criar(conta);

            var response = new ContaResponse()
            {
                Id = conta.Id,
                AgenciaId = conta.AgenciaId,
                ClienteNome = conta.Cliente.Nome
            };

            _logger.LogInformation("Método Criar() retornando {response}", JsonConvert.SerializeObject(response));

            return response;
        }

        public async Task<bool> ValidaContaExiste(int id)
        {
            _logger.LogInformation("Validando se conta Id:{id} existe no banco de dados...", id);

            return await _contaRepository.Pesquisar(id) != null;
        }

        public async Task<bool> ValidaSenhaCorreta(int contaId, string senha)
        {
            _logger.LogInformation("Validando se conta Id:{id} existe no banco de dados...", contaId);

            var conta = await _contaRepository.Pesquisar(contaId);

            _logger.LogInformation("Banco de dados retornou conta {conta}", JsonConvert.SerializeObject(conta));
            _logger.LogInformation("Verificando se a conta Id:{id} possui a senha {senha}", contaId, senha);

            return CriptografiaUtil.VerificaSenhaCriptografada(conta.Senha, senha);
        }

        public async Task<bool> ValidaEmailJaExiste(string email)
        {
            _logger.LogInformation("Validando se e-mail {email} existe no banco de dados...", email);

            return await _contaRepository.PesquisarPorEmailCliente(email) != null;
        }
    }
}
