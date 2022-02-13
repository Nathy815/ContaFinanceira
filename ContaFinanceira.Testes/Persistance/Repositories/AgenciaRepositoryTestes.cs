using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.Persistance.Repositories
{
    public class AgenciaRepositoryTestes : SqlServerContextInMemory
    {
        private IAgenciaRepository _agenciaRepository;

        public AgenciaRepositoryTestes()
        {
            //Arrange
            _agenciaRepository = GetAgenciaRepository();
        }

        [Fact]
        public async Task Listar_Sucesso()
        {
            //Act
            var result = await _agenciaRepository.Listar();

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(1, result.First().Id);
            Assert.Equal(2, result.Last().Id);
            Assert.Equal("Agência 1", result.First().Nome);
            Assert.Equal("Agência 2", result.Last().Nome);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Pesquisar_Sucesso(int id)
        {
            //Act
            var result = await _agenciaRepository.Pesquisar(id);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Pesquisar_Erro(int id)
        {
            //Act
            var result = await _agenciaRepository.Pesquisar(id);

            //Assert
            Assert.Null(result);
        }
    }
}
