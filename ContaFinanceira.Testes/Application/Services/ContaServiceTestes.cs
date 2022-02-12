using ContaFinanceira.Application.Services;
using ContaFinanceira.Domain.Entities;
using ContaFinanceira.Domain.Enum;
using ContaFinanceira.Domain.Interfaces.Repositories;
using ContaFinanceira.Domain.Requests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.Application.Services
{
    public class ContaServiceTestes
    {
        private readonly Mock<IContaRepository> _contaRepository;
        private List<Conta> _contas;

        public ContaServiceTestes()
        {
            _contaRepository = new Mock<IContaRepository>();
            _contas = new List<Conta>()
            {
                new Conta()
                {
                    AgenciaId = 1,
                    DataCriacao = DateTime.Now,
                    Id = 1,
                    Cliente = new Cliente()
                    {
                        Id = 1,
                        Nome = "Nathália Lopes",
                        CpfCnpj = "51865798916",
                        TipoPessoa = ePessoa.PessoaFisica,
                        ContaId = 1
                    },
                    Senha = "$2a$11$suS.0JwXHYjqT7ZIIAAMeuwnaxiZko4kfMOH8.CDOxQ9V0covPWdq"
                },
                new Conta()
                {
                    AgenciaId = 2,
                    DataCriacao = DateTime.Now,
                    Id = 2,
                    Senha = "$2a$12$2EXMNq57lYuw7ZAY7Stc/ulIoePfxKrcv1SxOtjtkpOJ1pxTDHTue"
                }
            };
        }

        [Fact]
        public async Task Criar_Sucesso_ComDeposito()
        {
            //Arrange
            var request = new ContaRequest()
            { 
                AgenciaId = 1,
                CpfCnpj = "51865798916",
                NomeCliente = "Nathália Lopes",
                Senha = "12345",
                DepositoInicial = 10M,
                TipoPessoa = ePessoa.PessoaFisica
            };

            _contaRepository
                .Setup(x => x.Criar(It.IsAny<Conta>()))
                .ReturnsAsync(_contas.First());

            var service = new ContaService(_contaRepository.Object);

            //Act
            var result = await service.Criar(request);

            //Assert
            Assert.Equal(1, result.Id);
            Assert.Equal(request.AgenciaId, result.AgenciaId);
            Assert.Equal(request.NomeCliente, result.ClienteNome);
        }

        [Fact]
        public async Task Criar_Sucesso_SemDeposito()
        {
            //Arrange
            var request = new ContaRequest()
            {
                AgenciaId = 1,
                CpfCnpj = "51865798916",
                NomeCliente = "Nathália Lopes",
                Senha = "12345",
                TipoPessoa = ePessoa.PessoaFisica
            };

            _contaRepository
                .Setup(x => x.Criar(It.IsAny<Conta>()))
                .ReturnsAsync(_contas.First());

            var service = new ContaService(_contaRepository.Object);

            //Act
            var result = await service.Criar(request);

            //Assert
            Assert.Equal(1, result.Id);
            Assert.Equal(request.AgenciaId, result.AgenciaId);
            Assert.Equal(request.NomeCliente, result.ClienteNome);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ValidaContaExiste_Sucesso(int id)
        {
            //Arrange
            _contaRepository
                .Setup(x => x.Pesquisar(id))
                .ReturnsAsync(_contas.Where(x => x.Id == id).FirstOrDefault());

            var service = new ContaService(_contaRepository.Object);

            //Act
            var result = await service.ValidaContaExiste(id);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public async Task ValidaContaExiste_Erro(int id)
        {
            //Arrange
            _contaRepository
                .Setup(x => x.Pesquisar(id))
                .ReturnsAsync(_contas.Where(x => x.Id == id).FirstOrDefault());

            var service = new ContaService(_contaRepository.Object);

            //Act
            var result = await service.ValidaContaExiste(id);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(1, "12345")]
        [InlineData(2, "23456")]
        public async Task ValidaSenhaCorreta_Sucesso(int id, string senha)
        {
            //Arrange
            _contaRepository
                .Setup(x => x.Pesquisar(id))
                .ReturnsAsync(_contas.Where(x => x.Id == id).FirstOrDefault());

            var service = new ContaService(_contaRepository.Object);

            //Act
            var result = await service.ValidaSenhaCorreta(id, senha);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(1, "23456")]
        [InlineData(2, "12345")]
        public async Task ValidaSenhaCorreta_Erro_SenhaIncorreta(int id, string senha)
        {
            //Arrange
            _contaRepository
                .Setup(x => x.Pesquisar(id))
                .ReturnsAsync(_contas.Where(x => x.Id == id).FirstOrDefault());

            var service = new ContaService(_contaRepository.Object);

            //Act
            var result = await service.ValidaSenhaCorreta(id, senha);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(3, "34567")]
        [InlineData(4, "45678")]
        public async Task ValidaSenhaCorreta_Erro_UsuarioNaoEncontrado(int id, string senha)
        {
            //Arrange
            _contaRepository
                .Setup(x => x.Pesquisar(id))
                .ReturnsAsync(_contas.Where(x => x.Id == id).FirstOrDefault());

            var service = new ContaService(_contaRepository.Object);

            //Act and Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => service.ValidaSenhaCorreta(id, senha));
        }
    }
}
