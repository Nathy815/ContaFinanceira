using ContaFinanceira.API.Controllers;
using ContaFinanceira.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContaFinanceira.Testes.API.Controllers
{
    public class TiposPessoasControllerTestes
    {
        private readonly Mock<ILogger<TiposPessoasController>> _logger;

        public TiposPessoasControllerTestes()
        {
            _logger = new Mock<ILogger<TiposPessoasController>>();
        }

        [Fact]
        public async Task ListarTipoPessoa_Sucesso()
        {
            //Arrange
            var controller = new TiposPessoasController(_logger.Object);

            //Act
            var result = await controller.ListarTipoPessoa();

            //Assert
            var model = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(200, model.StatusCode);
            var lista = Assert.IsAssignableFrom<List<TipoPessoaResponse>>(model.Value);
            Assert.Equal(1, lista.First().Id);
            Assert.Equal("PessoaFisica", lista.First().Nome);
            Assert.Equal(2, lista.Last().Id);
            Assert.Equal("PessoaJuridica", lista.Last().Nome);
        }
    }
}
