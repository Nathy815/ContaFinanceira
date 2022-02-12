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
    public class AutenticationRequestValidationTestes
    {
        private readonly Mock<IContaService> _contaService;
        private AutenticacaoRequest _request;

        public AutenticationRequestValidationTestes()
        {
            _contaService = new Mock<IContaService>();
            _request = new AutenticacaoRequest()
            {
                ContaId = 1,
                Senha = "12345"
            };
        }

        [Fact]
        public void AutenticationRequestValidation_Sucesso()
        {
            //Arrange
            _contaService
                .Setup(x => x.ValidaContaExiste(_request.ContaId))
                .ReturnsAsync(true);

            _contaService
                .Setup(x => x.ValidaSenhaCorreta(_request.ContaId, _request.Senha))
                .ReturnsAsync(true);

            var validator = new AutenticacaoRequestValidation(_contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void AutenticationRequestValidation_Erro_ContaNaoInformada()
        {
            //Arrange
            var request = new AutenticacaoRequest()
            {
                Senha = "12345"
            };

            _contaService
                .Setup(x => x.ValidaContaExiste(request.ContaId))
                .ReturnsAsync(true);

            _contaService
                .Setup(x => x.ValidaSenhaCorreta(request.ContaId, request.Senha))
                .ReturnsAsync(true);

            var validator = new AutenticacaoRequestValidation(_contaService.Object);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("ContaId") && x.ErrorMessage.Equals("Por favor, informe a conta.")).FirstOrDefault());
        }

        [Fact]
        public void AutenticationRequestValidation_Erro_ContaNaoExiste()
        {
            //Arrange
            _contaService
                .Setup(x => x.ValidaContaExiste(_request.ContaId))
                .ReturnsAsync(false);

            _contaService
                .Setup(x => x.ValidaSenhaCorreta(_request.ContaId, _request.Senha))
                .ReturnsAsync(true);

            var validator = new AutenticacaoRequestValidation(_contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("ContaId") && x.ErrorMessage.Equals("Conta inválida.")).FirstOrDefault());
        }

        [Fact]
        public void AutenticationRequestValidation_Erro_SenhaNaoInformada()
        {
            //Arrange
            var request = new AutenticacaoRequest()
            {
                ContaId = 1
            };

            _contaService
                .Setup(x => x.ValidaContaExiste(request.ContaId))
                .ReturnsAsync(true);

            _contaService
                .Setup(x => x.ValidaSenhaCorreta(request.ContaId, request.Senha))
                .ReturnsAsync(true);

            var validator = new AutenticacaoRequestValidation(_contaService.Object);

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("Senha") && x.ErrorMessage.Equals("Por favor, informe a senha.")).FirstOrDefault());
        }

        [Fact]
        public void AutenticationRequestValidation_Erro_SenhaIncorreta()
        {
            //Arrange
            _contaService
                .Setup(x => x.ValidaContaExiste(_request.ContaId))
                .ReturnsAsync(true);

            _contaService
                .Setup(x => x.ValidaSenhaCorreta(_request.ContaId, _request.Senha))
                .ReturnsAsync(false);

            var validator = new AutenticacaoRequestValidation(_contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("Senha") && x.ErrorMessage.Equals("Senha inválida.")).FirstOrDefault());
        }
    }
}
