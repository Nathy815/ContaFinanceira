using ContaFinanceira.Domain.Enum;
using ContaFinanceira.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContaFinanceira.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TiposPessoasController : ControllerBase
    {
        private readonly ILogger<TiposPessoasController> _logger;

        public TiposPessoasController(ILogger<TiposPessoasController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarTipoPessoa()
        {
            _logger.LogInformation("Iniciando requisição de ListarTipoPessoa()...");

            var names = Enum.GetNames(typeof(ePessoa));
            var result = new List<TipoPessoaResponse>();
            var count = 1;

            foreach (var name in names)
            {
                result.Add(new TipoPessoaResponse() { Id = count, Nome = name });
                count++;
            }

            _logger.LogInformation("Requisição ListarTipoPessoa() processada com sucesso! Retorno: {result}", JsonConvert.SerializeObject(result));

            return new OkObjectResult(result);
        }
    }
}
