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

        public ClienteService(IClienteRepository clienteRepository,
                              IConfiguration configuration)
        {
            _clienteRepository = clienteRepository;
            _configuration = configuration;
        }

        public async Task<TokenResponse> Autenticar(AutenticacaoRequest request)
        {
            var _cliente = await _clienteRepository.PesquisarPorConta(request.ContaId);

            if (CriptografiaUtil.VerificaSenhaCriptografada(_cliente.Conta.Senha, request.Senha))
            {
                var _token = await GerarToken(_cliente, request.Senha);

                return _token;
            }
            else
                return null;
        }

        private Task<TokenResponse> GerarToken(Cliente cliente, string senha)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_configuration.GetSection("Authentication:SecurityKey").Value);

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

            return Task.Run(() =>
            {
                return new TokenResponse()
                {
                    Token = handler.WriteToken(token),
                    Validation = token.ValidTo
                };
            });
        }
    }
}
