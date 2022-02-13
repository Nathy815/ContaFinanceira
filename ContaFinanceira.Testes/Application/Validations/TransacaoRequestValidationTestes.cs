using ContaFinanceira.Application.Validations;
using ContaFinanceira.Domain.Interfaces.Services;
using ContaFinanceira.Domain.Requests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.Application.Validations
{
    public class TransacaoRequestValidationTestes
    {
        private readonly Mock<ITransacaoService> _transacaoService;
        private readonly Mock<IContaService> _contaService;

        public TransacaoRequestValidationTestes()
        {
            _transacaoService = new Mock<ITransacaoService>();
            _contaService = new Mock<IContaService>();
        }

        [Fact]
        public void TransacaoRequestValidation_Sucesso_Deposito()
        {
            //Arrange
            var request = new TransacaoRequest() { Valor = 10M };
            request.setConta(1);

            _transacaoService
                .Setup(x => x.ValidarSaldoSuficiente(request.ContaId, request.Valor))
                .Returns(true);

            _contaService
                .Setup(x => x.ValidaContaExiste(request.ContaId))
                .ReturnsAsync(true);

            var validator = new TransacaoRequestValidation(_transacaoService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void TransacaoRequestValidation_Sucesso_Saque()
        {
            //Arrange
            var request = new TransacaoRequest() { Valor = -10M };
            request.setConta(1);

            _transacaoService
                .Setup(x => x.ValidarSaldoSuficiente(request.ContaId, request.Valor))
                .Returns(true);

            _contaService
                .Setup(x => x.ValidaContaExiste(request.ContaId))
                .ReturnsAsync(true);

            var validator = new TransacaoRequestValidation(_transacaoService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void TransacaoRequestValidation_Erro_ValorNaoInformado()
        {
            //Arrange
            var request = new TransacaoRequest();
            request.setConta(1);

            _transacaoService
                .Setup(x => x.ValidarSaldoSuficiente(request.ContaId, request.Valor))
                .Returns(true);

            _contaService
                .Setup(x => x.ValidaContaExiste(request.ContaId))
                .ReturnsAsync(true);

            var validator = new TransacaoRequestValidation(_transacaoService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("Valor") && x.ErrorMessage.Equals("Por favor, informe um valor.")).FirstOrDefault());
        }

        [Fact]
        public void TransacaoRequestValidation_Erro_SaldoInsuficiente()
        {
            //Arrange
            var request = new TransacaoRequest() { Valor = -10M };
            request.setConta(1);

            _transacaoService
                .Setup(x => x.ValidarSaldoSuficiente(request.ContaId, request.Valor))
                .Returns(false);

            _contaService
                .Setup(x => x.ValidaContaExiste(request.ContaId))
                .ReturnsAsync(true);

            var validator = new TransacaoRequestValidation(_transacaoService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("Valor") && x.ErrorMessage.Equals("Saldo insuficiente.")).FirstOrDefault());
        }
    }
}
