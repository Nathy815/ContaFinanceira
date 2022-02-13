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
    public class TransacaoRepositoryTestes : SqlServerContextInMemory
    {
        private ITransacaoRepository _transacaoRepository;

        public TransacaoRepositoryTestes()
        {
            //Arrange
            _transacaoRepository = GetTransacaoRepository();
        }

        [Fact]
        public async Task Criar_Sucesso()
        {
            //Arrange
            var data = DateTime.Now;
            var transacao = new Transacao() { Id = 5, ContaId = 1, Data = data, Valor = 10M };

            //Act
            var result = await _transacaoRepository.Criar(transacao);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal(1, result.ContaId);
            Assert.Equal(data, result.Data);
            Assert.Equal(10M, result.Valor);
        }

        [Fact]
        public async Task Criar_Erro()
        {
            //Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _transacaoRepository.Criar(null));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Listar_Sucesso(int contaId)
        {
            //Act
            var result = await _transacaoRepository.Listar(contaId);

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result.Where(x => x.ContaId != contaId).ToList());
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task Listar_Erro(int contaId)
        {
            //Act
            var result = await _transacaoRepository.Listar(contaId);

            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Saldo_Sucesso(int contaId)
        {
            //Act
            var result = _transacaoRepository.Saldo(contaId);

            //Assert
            Assert.True(result > 0);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public void Saldo_Erro(int contaId)
        {
            //Act
            var result = _transacaoRepository.Saldo(contaId);

            //Assert
            Assert.False(result > 0);
        }
    }
}
