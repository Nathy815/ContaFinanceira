using ContaFinanceira.Application.Services;
using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.Application.Services
{
    public class AgenciaServiceTestes
    {
        private readonly Mock<IAgenciaRepository> _agenciaRepository;
        private readonly Mock<ILogger<AgenciaService>> _logger;
        private List<Agencia> _agencias;

        public AgenciaServiceTestes()
        {
            _agenciaRepository = new Mock<IAgenciaRepository>();
            _agencias = new List<Agencia>()
            {
                new Agencia() { Id = 1, Nome = "Agência 1" },
                new Agencia() { Id = 2, Nome = "Agência 2" }
            };
            _logger = new Mock<ILogger<AgenciaService>>();
        }

        [Fact]
        public async Task Listar_Sucesso()
        {
            //Arrange
            _agenciaRepository
                .Setup(x => x.Listar())
                .ReturnsAsync(_agencias);

            var service = new AgenciaService(_agenciaRepository.Object, _logger.Object);

            //Act
            var result = await service.Listar();

            //Assert
            Assert.Equal(result.First().Id, _agencias.First().Id);
            Assert.Equal(result.First().Nome, _agencias.First().Nome);
            Assert.Equal(result.Last().Id, _agencias.Last().Id);
            Assert.Equal(result.Last().Nome, _agencias.Last().Nome);
        }

        [Fact]
        public async Task Listar_Erro_Nulo()
        {
            //Arrange
            _agenciaRepository
                .Setup(x => x.Listar())
                .ReturnsAsync(It.IsAny<List<Agencia>>());

            var service = new AgenciaService(_agenciaRepository.Object, _logger.Object);

            //Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() =>  service.Listar());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ValidaAgencia_Sucesso(int id)
        {
            //Arrange
            _agenciaRepository
                .Setup(x => x.Pesquisar(id))
                .ReturnsAsync(_agencias.Where(x => x.Id == id).FirstOrDefault());

            var service = new AgenciaService(_agenciaRepository.Object, _logger.Object);

            //Act
            var result = await service.ValidaAgencia(id);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        public async Task ValidaAgencia_Erro(int id)
        {
            //Arrange
            _agenciaRepository
                .Setup(x => x.Pesquisar(id))
                .ReturnsAsync(_agencias.Where(x => x.Id == id).FirstOrDefault());

            var service = new AgenciaService(_agenciaRepository.Object, _logger.Object);

            //Act
            var result = await service.ValidaAgencia(id);

            //Assert
            Assert.False(result);
        }
    }
}
