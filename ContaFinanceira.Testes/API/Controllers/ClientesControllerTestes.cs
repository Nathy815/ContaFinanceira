using ContaFinanceira.API.Controllers;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.API.Controllers
{
    public class ClientesControllerTestes
    {
        private readonly Mock<IClienteService> _clienteService;

        public ClientesControllerTestes()
        {
            _clienteService = new Mock<IClienteService>();
        }

        [Fact]
        public async Task Autenticar_Sucesso()
        {
            //Arrange
            var response = new TokenResponse() { Token = "abc123", Validation = DateTime.Now.AddHours(4) };

            _clienteService
                .Setup(x => x.Autenticar(It.IsAny<AutenticacaoRequest>()))
                .ReturnsAsync(response);

            var controller = new ClientesController(_clienteService.Object);

            //Act
            var result = await controller.Autenticar(It.IsAny<AutenticacaoRequest>());

            //Assert
            var model = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(200, model.StatusCode);
            var token = Assert.IsAssignableFrom<TokenResponse>(model.Value);
            Assert.Equal(response.Token, token.Token);
            Assert.Equal(response.Validation, token.Validation);
        }

        [Fact]
        public async Task Autenticar_Erro_Valicacao()
        {
            //Arrange
            _clienteService
                .Setup(x => x.Autenticar(It.IsAny<AutenticacaoRequest>()))
                .ThrowsAsync(new ValidationException("Conta inválida."));

            var controller = new ClientesController(_clienteService.Object);

            //Act
            var result = await controller.Autenticar(It.IsAny<AutenticacaoRequest>());

            //Assert
            var model = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal(400, model.StatusCode);
            Assert.Equal("Conta inválida.", model.Value);
        }

        [Fact]
        public async Task Autenticar_Erro()
        {
            //Arrange
            _clienteService
                .Setup(x => x.Autenticar(It.IsAny<AutenticacaoRequest>()))
                .ThrowsAsync(new Exception("Erro ao comunicar-se com o banco de dados"));

            var controller = new ClientesController(_clienteService.Object);

            //Act
            var result = await controller.Autenticar(It.IsAny<AutenticacaoRequest>());

            //Assert
            var model = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.Equal("Erro ao comunicar-se com o banco de dados", model.Value);
        }
    }
}
