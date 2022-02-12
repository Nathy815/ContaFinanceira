using ContaFinanceira.Application.Services;
using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Enum;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Requests;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.Application.Services
{
    public class ClienteServiceTestes
    {
        private readonly Mock<IClienteRepository> _clienteRepository;
        private readonly Mock<IConfiguration> _configuration;
        private Cliente _cliente;

        public ClienteServiceTestes()
        {
            _clienteRepository = new Mock<IClienteRepository>();
            _configuration = new Mock<IConfiguration>();
            _cliente = new Cliente()
            {
                Id = 1,
                Nome = "Nathália Lopes",
                ContaId = 1,
                Conta = new Conta()
                {
                    Id = 1,
                    AgenciaId = 1,
                    DataCriacao = DateTime.Now,
                    Senha = "12345"
                },
                CpfCnpj = "51865798916",
                TipoPessoa = ePessoa.PessoaFisica
            };
        }

        [Fact]
        public async Task Autenticar_Sucesso()
        {
            //Arrange
            var request = new AutenticacaoRequest() { ContaId = 1, Senha = "12345" };

            _clienteRepository
                .Setup(x => x.PesquisarPorConta(request.ContaId))
                .ReturnsAsync(_cliente);

            _configuration
                .Setup(x => x.GetSection(It.IsAny<string>()).Value)
                .Returns("eiVDKkYtSmFOZFJnVWtYcA==");

            var service = new ClienteService(_clienteRepository.Object, _configuration.Object);

            //Act
            var result = await service.Autenticar(request);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Autenticar_Erro_ClienteNulo()
        {
            //Arrange
            var request = new AutenticacaoRequest() { ContaId = 1, Senha = "12345" };

            _clienteRepository
                .Setup(x => x.PesquisarPorConta(request.ContaId))
                .ReturnsAsync(It.IsAny<Cliente>());

            _configuration
                .Setup(x => x.GetSection(It.IsAny<string>()).Value)
                .Returns("Q29udGFGaW5hbmNlaXJh");

            var service = new ClienteService(_clienteRepository.Object, _configuration.Object);

            //Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => service.Autenticar(request));
        }

        [Fact]
        public async Task Autenticar_Erro_SecurityKeyNulo()
        {
            //Arrange
            var request = new AutenticacaoRequest() { ContaId = 1, Senha = "12345" };

            _clienteRepository
                .Setup(x => x.PesquisarPorConta(request.ContaId))
                .ReturnsAsync(_cliente);

            _configuration
                .Setup(x => x.GetSection(It.IsAny<string>()).Value)
                .Returns(It.IsAny<string>());

            var service = new ClienteService(_clienteRepository.Object, _configuration.Object);

            //Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.Autenticar(request));
        }
    }
}
