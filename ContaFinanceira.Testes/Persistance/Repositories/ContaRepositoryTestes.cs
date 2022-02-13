using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.Persistance.Repositories
{
    public class ContaRepositoryTestes : SqlServerContextInMemory
    {
        private IContaRepository _contaRepository;

        public ContaRepositoryTestes()
        {
            //Arrange
            _contaRepository = GetContaRepository();
        }

        [Fact]
        public async Task Criar_Sucesso()
        {
            //Arrange
            var conta = new Conta()
            {
                Id = 3,
                AgenciaId = 1,
                DataCriacao = DateTime.Now,
                Senha = "12345",
                Cliente = new Cliente() 
                { 
                    Id = 3,
                    ContaId = 3,
                    CpfCnpj = "12345678901",
                    Nome = "Cliente 3",
                    TipoPessoa = Domain.Enum.ePessoa.PessoaFisica
                }
            };

            //Act
            var result = await _contaRepository.Criar(conta);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
            Assert.Equal(1, result.AgenciaId);
            Assert.Equal("12345", result.Senha);
        }

        [Fact]
        public async Task Criar_Erro()
        {
            //Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _contaRepository.Criar(null));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Pesquisar_Sucesso(int contaId)
        {
            //Act
            var result = await _contaRepository.Pesquisar(contaId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(contaId, result.Id);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Pesquisar_Erro(int contaId)
        {
            //Act
            var result = await _contaRepository.Pesquisar(contaId);

            //Assert
            Assert.Null(result);
        }
    }
}
