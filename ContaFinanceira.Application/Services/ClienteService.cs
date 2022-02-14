using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using ContaFinanceira.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContaFinanceira.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(IClienteRepository clienteRepository,
                              IConfiguration configuration,
                              ILogger<ClienteService> logger)
        {
            _clienteRepository = clienteRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<TokenResponse> Autenticar(AutenticacaoRequest request)
        {
            _logger.LogInformation("Buscando cliente no banco de dados pela conta Id:{id}...", request.ContaId);

            var _cliente = await _clienteRepository.PesquisarPorConta(request.ContaId);

            _logger.LogInformation("Banco de dados retornou cliente {cliente}", JsonConvert.SerializeObject(_cliente));

            return await GerarToken(_cliente, request.Senha);
        }

        private Task<TokenResponse> GerarToken(Cliente cliente, string senha)
        {
            _logger.LogInformation("Criando token para cliente {cliente} com senha {senha}", JsonConvert.SerializeObject(cliente), senha);

            var handler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_configuration.GetSection("Authentication:SecurityKey").Value);

            _logger.LogDebug("SecurityKey {key} recuperada do appsettings", key);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, cliente.Conta.Id.ToString()),
                    new Claim(ClaimTypes.Name, cliente.Nome)
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handler.CreateToken(descriptor);

            var response = new TokenResponse()
            {
                Token = handler.WriteToken(token),
                Validation = token.ValidTo
            };

            _logger.LogInformation("Método GerarToken() retornando {retorno}", JsonConvert.SerializeObject(response));

            return Task.Run(() => { return response; });
        }
    }
}