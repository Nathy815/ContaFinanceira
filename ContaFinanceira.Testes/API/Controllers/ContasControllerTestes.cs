using ContaFinanceira.API.Controllers;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using ContaFinanceira.Domain.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.API.Controllers
{
    public class ContasControllerTestes
    {
        private readonly Mock<IContaService> _contaService;

        public ContasControllerTestes()
        {
            _contaService = new Mock<IContaService>();
        }

        [Fact]
        public async Task Criar_Sucesso()
        {
            //Arrange
            var response = new ContaResponse() { Id = 1, ClienteNome = "Nathália Lopes", AgenciaId = 1 };

            _contaService
                .Setup(x => x.Criar(It.IsAny<ContaRequest>()))
                .ReturnsAsync(response);

            var controller = new ContasController(_contaService.Object);

            //Act
            var result = await controller.Criar(It.IsAny<ContaRequest>());

            //Assert
            var model = Assert.IsAssignableFrom<CreatedResult>(result);
            Assert.Equal(201, model.StatusCode);
            var conta = Assert.IsAssignableFrom<ContaResponse>(model.Value);
            Assert.Equal(response.Id, conta.Id);
            Assert.Equal(response.ClienteNome, conta.ClienteNome);
            Assert.Equal(response.AgenciaId, conta.AgenciaId);
        }

        [Fact]
        public async Task Criar_Erro_Validacao()
        {
            //Arrange
            _contaService
                .Setup(x => x.Criar(It.IsAny<ContaRequest>()))
                .ThrowsAsync(new ValidationException("Conta inválida."));

            var controller = new ContasController(_contaService.Object);

            //Act
            var result = await controller.Criar(It.IsAny<ContaRequest>());

            //Assert
            var model = Assert.IsAssignableFrom<BadRequestObjectResult>(result);
            Assert.Equal(400, model.StatusCode);
            Assert.Equal("Conta inválida.", model.Value);
        }

        [Fact]
        public async Task Criar_Erro()
        {
            //Arrange
            _contaService
                .Setup(x => x.Criar(It.IsAny<ContaRequest>()))
                .ThrowsAsync(new Exception("Erro ao comunicar-se ao banco de dados."));

            var controller = new ContasController(_contaService.Object);

            //Act
            var result = await controller.Criar(It.IsAny<ContaRequest>());

            //Assert
            var model = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.Equal("Erro ao comunicar-se ao banco de dados.", model.Value);
        }
    }
}
