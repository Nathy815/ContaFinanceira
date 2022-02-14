using ContaFinanceira.Application.Services;
using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Requests;
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
    public class TransacaoServiceTestes
    {
        private readonly Mock<ITransacaoRepository> _transacaoRepository;
        private readonly Mock<ILogger<TransacaoService>> _logger;
        private List<Transacao> _transacoes;

        public TransacaoServiceTestes()
        {
            _transacaoRepository = new Mock<ITransacaoRepository>();
            _transacoes = new List<Transacao>()
            {
                new Transacao()
                {
                    Id = 1,
                    ContaId = 1,
                    Data = DateTime.Now,
                    Valor = 30M
                },
                new Transacao()
                {
                    Id = 2,
                    ContaId = 2,
                    Data = DateTime.Now,
                    Valor = 10M
                }
            };
            _logger = new Mock<ILogger<TransacaoService>>();
        }

        [Fact]
        public async Task Adicionar_Sucesso_Deposito()
        {
            //Arrange
            var request = new TransacaoRequest()
            {
                Valor = 10M
            };
            request.setConta(1);

            _transacaoRepository
                .Setup(x => x.Criar(It.IsAny<Transacao>()))
                .Verifiable();

            _transacaoRepository
                .Setup(x => x.Listar(It.IsAny<int>()))
                .ReturnsAsync(_transacoes);

            var service = new TransacaoService(_transacaoRepository.Object, _logger.Object);

            //Act
            var result = await service.Adicionar(request);

            //Assert
            _transacaoRepository
                .Verify(x => x.Criar(It.IsAny<Transacao>()), Times.Once);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Adicionar_Sucesso_Saque()
        {
            //Arrange
            var request = new TransacaoRequest()
            {
                Valor = -10M
            };
            request.setConta(1);

            _transacaoRepository
                .Setup(x => x.Criar(It.IsAny<Transacao>()))
                .Verifiable();

            _transacaoRepository
                .Setup(x => x.Listar(It.IsAny<int>()))
                .ReturnsAsync(_transacoes);

            var service = new TransacaoService(_transacaoRepository.Object, _logger.Object);

            //Act
            var result = await service.Adicionar(request);

            //Assert
            _transacaoRepository
                .Verify(x => x.Criar(It.IsAny<Transacao>()), Times.Once);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Adicionar_Erro_ListarNulo()
        {
            //Arrange
            var request = new TransacaoRequest()
            {
                Valor = 10M
            };
            request.setConta(1);

            _transacaoRepository
                .Setup(x => x.Criar(It.IsAny<Transacao>()))
                .Verifiable();

            _transacaoRepository
                .Setup(x => x.Listar(It.IsAny<int>()))
                .ReturnsAsync(It.IsAny<List<Transacao>>());

            var service = new TransacaoService(_transacaoRepository.Object, _logger.Object);

            //Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => service.Adicionar(request));
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(1, -20)]
        [InlineData(2, 22)]
        [InlineData(2, -7)]
        public void ValidarSaldoSuficiente_Sucesso(int contaId, decimal valor)
        {
            //Arrange
            _transacaoRepository
                .Setup(x => x.Saldo(contaId))
                .Returns(_transacoes.Where(x => x.ContaId == contaId).Sum(x => x.Valor));

            var service = new TransacaoService(_transacaoRepository.Object, _logger.Object);

            //Act
            var result = service.ValidarSaldoSuficiente(contaId, valor);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(1, -40)]
        [InlineData(2, -12)]
        public void ValidarSaldoSuficiente_Erro_SaldoInsuficiente(int contaId, decimal valor)
        {
            //Arrange
            _transacaoRepository
                .Setup(x => x.Saldo(contaId))
                .Returns(_transacoes.Where(x => x.ContaId == contaId).Sum(x => x.Valor));

            var service = new TransacaoService(_transacaoRepository.Object, _logger.Object);

            //Act
            var result = service.ValidarSaldoSuficiente(contaId, valor);

            //Assert
            Assert.False(result);
        }
    }
}
