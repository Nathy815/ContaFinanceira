using ContaFinanceira.API.Controllers;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.API.Controllers
{
    public class AgenciasControllerTestes
    {
        private readonly Mock<IAgenciaService> _agenciaService;
        private readonly Mock<ILogger<AgenciasController>> _logger;

        public AgenciasControllerTestes()
        {
            _agenciaService = new Mock<IAgenciaService>();
            _logger = new Mock<ILogger<AgenciasController>>();
        }

        [Fact]
        public async Task Listar_Sucesso()
        {
            //Arrange
            _agenciaService
                .Setup(x => x.Listar())
                .ReturnsAsync(new List<AgenciaResponse>() { new AgenciaResponse() { Id = 1, Nome = "Agência 1" } });

            var _controller = new AgenciasController(_agenciaService.Object, _logger.Object);

            //Act
            var result = await _controller.Listar();

            //Assert
            var model = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(200, model.StatusCode);
            var lista = Assert.IsAssignableFrom<List<AgenciaResponse>>(model.Value);
            Assert.Equal(1, lista.First().Id);
            Assert.Equal("Agência 1", lista.First().Nome);
        }

        [Fact]
        public async Task Listar_Erro()
        {
            //Arrange
            _agenciaService
                .Setup(x => x.Listar())
                .ThrowsAsync(new Exception("Erro ao comunicar-se com o banco de dados"));

            var controller = new AgenciasController(_agenciaService.Object, _logger.Object);

            //Act
            var result = await controller.Listar();

            //Assert
            var model = Assert.IsAssignableFrom<ObjectResult>(result);
            Assert.Equal(500, model.StatusCode);
            Assert.Equal("Erro ao comunicar-se com o banco de dados", model.Value);
        }
    }
}
