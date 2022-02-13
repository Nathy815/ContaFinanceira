using ContaFinanceira.Domain.Enum;
using ContaFinanceira.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarTipoPessoa()
        {
            var names = Enum.GetNames(typeof(ePessoa));
            var result = new List<TipoPessoaResponse>();
            var count = 1;

            foreach (var name in names)
            {
                result.Add(new TipoPessoaResponse() { Id = count, Nome = name });
                count++;
            }

            return new OkObjectResult(result);
        }
    }
}
