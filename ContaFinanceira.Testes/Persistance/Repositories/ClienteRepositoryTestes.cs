using ContaFinanceira.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.Persistance.Repositories
{
    public class ClienteRepositoryTestes : SqlServerContextInMemory
    {
        private IClienteRepository _clienteRepository;

        public ClienteRepositoryTestes()
        {
            //Arrange
            _clienteRepository = GetClienteRepository();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task PesquisarPorConta_Sucesso(int contaId)
        {
            //Act
            var result = await _clienteRepository.PesquisarPorConta(contaId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(contaId, result.Conta.Id);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task PesquisarPorConta_Erro(int contaId)
        {
            //Act
            var result = await _clienteRepository.PesquisarPorConta(contaId);

            //Assert
            Assert.Null(result);
        }
    }
}
