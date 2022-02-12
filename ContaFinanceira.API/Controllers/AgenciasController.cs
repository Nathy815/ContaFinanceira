using ContaFinanceira.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ContaFinanceira.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgenciasController : ControllerBase
    {
        private readonly IAgenciaService _service;

        public AgenciasController(IAgenciaService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var result = await _service.Listar();

                return new OkObjectResult(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
