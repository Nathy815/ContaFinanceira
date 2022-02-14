using ContaFinanceira.Application.Validations;
using ContaFinanceira.Domain.Enum;
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
    public class ContaRequestValidationTestes
    {
        private readonly Mock<IAgenciaService> _agenciaService;
        private readonly Mock<IContaService> _contaService;

        public ContaRequestValidationTestes()
        {
            _agenciaService = new Mock<IAgenciaService>();
            _contaService = new Mock<IContaService>();
            
        }

        [Fact]
        public void ContaRequestValidation_Sucesso_PessoaFisica_ComDeposito()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 1,
                CpfCnpj = "51865798916",
                Email = "nathalialcoimbra@gmail.com",
                Senha = "12345",
                TipoPessoa = ePessoa.PessoaFisica,
                DepositoInicial = 10M
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ContaRequestValidation_Sucesso_PessoaJuridica_SemDeposito()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 1,
                CpfCnpj = "71812201000115",
                Email = "nathalialcoimbra@gmail.com",
                Senha = "12345",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void ContaRequestValidation_Erro_NomeClienteNaoInformado()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                AgenciaId = 1,
                CpfCnpj = "71812201000115",
                Email = "nathalialcoimbra@gmail.com",
                Senha = "12345",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("NomeCliente") && x.ErrorMessage.Equals("Por favor, informe o nome do cliente.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_NomeClienteInvalido()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes Coimbra Nathália Lopes Coimbra Nathália Lopes Coimbra",
                AgenciaId = 1,
                CpfCnpj = "71812201000115",
                Email = "nathalialcoimbra@gmail.com",
                Senha = "12345",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("NomeCliente") && x.ErrorMessage.Equals("O nome do cliente não pode ser maior que 50 caracteres.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_AgenciaNaoInformado()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                CpfCnpj = "71812201000115",
                Email = "nathalialcoimbra@gmail.com",
                Senha = "12345",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("AgenciaId") && x.ErrorMessage.Equals("Por favor, informe uma agência.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_AgenciaNaoEncontrada()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 2,
                CpfCnpj = "71812201000115",
                Email = "nathalialcoimbra@gmail.com",
                Senha = "12345",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(false);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("AgenciaId") && x.ErrorMessage.Equals("Agência inválida.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_TipoPessoaInvalido()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 1,
                CpfCnpj = "71812201000115",
                Email = "nathalialcoimbra@gmail.com",
                Senha = "12345",
                TipoPessoa = (ePessoa)3
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("TipoPessoa") && x.ErrorMessage.Equals("Por favor, informe um valor válido de tipo de pessoa.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_CpfCnpjNaoInformado()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 2,
                Senha = "12345",
                Email = "nathalialcoimbra@gmail.com",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("CpfCnpj") && x.ErrorMessage.Equals("Por favor, informe um CPF/CNPJ.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_CpfInvalido()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 2,
                CpfCnpj = "71812201000115",
                Senha = "12345",
                Email = "nathalialcoimbra@gmail.com",
                TipoPessoa = ePessoa.PessoaFisica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("CpfCnpj") && x.ErrorMessage.Equals("CPF/CNPJ inválido.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_CnpjInvalido()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 2,
                CpfCnpj = "51865798916",
                Senha = "12345",
                Email = "nathalialcoimbra@gmail.com",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("CpfCnpj") && x.ErrorMessage.Equals("CPF/CNPJ inválido.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_EmailNaoInformado()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 1,
                CpfCnpj = "71812201000115",
                Senha = "12345",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("Email") && x.ErrorMessage.Equals("Por favor, informe um e-mail.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_EmailInvalido()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 1,
                CpfCnpj = "71812201000115",
                Email = "teste",
                Senha = "12345",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("Email") && x.ErrorMessage.Equals("E-mail inválido.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_SenhaNaoInformada()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 2,
                CpfCnpj = "71812201000115",
                Email = "nathalialcoimbra@gmail.com",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("Senha") && x.ErrorMessage.Equals("Por favor, informe uma senha.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_SenhaCurta()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 2,
                CpfCnpj = "71812201000115",
                Email = "nathalialcoimbra@gmail.com",
                Senha = "1234",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("Senha") && x.ErrorMessage.Equals("A senha não pode ser menor que 5 caracteres.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_SenhaLonga()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 2,
                CpfCnpj = "71812201000115",
                Email = "nathalialcoimbra@gmail.com",
                Senha = "12345678901",
                TipoPessoa = ePessoa.PessoaJuridica
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("Senha") && x.ErrorMessage.Equals("A senha não pode ser maior que 10 caracteres.")).FirstOrDefault());
        }

        [Fact]
        public void ContaRequestValidation_Erro_DepositoInvalido()
        {
            //Arrange
            var _request = new ContaRequest()
            {
                NomeCliente = "Nathália Lopes",
                AgenciaId = 2,
                CpfCnpj = "71812201000115",
                Email = "nathalialcoimbra@gmail.com",
                Senha = "1234567890",
                TipoPessoa = ePessoa.PessoaJuridica,
                DepositoInicial = 0M
            };

            _agenciaService
                .Setup(x => x.ValidaAgencia(_request.AgenciaId))
                .ReturnsAsync(true);

            var validator = new ContaRequestValidation(_agenciaService.Object, _contaService.Object);

            //Act
            var result = validator.Validate(_request);

            //Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
            Assert.NotNull(result.Errors.Where(x => x.PropertyName.Equals("DepositoInicial") && x.ErrorMessage.Equals("Você não pode sacar na abertura de conta.")).FirstOrDefault());
        }

    }
}
